using System.Net;
using System.Text.Json;
using GeminiAsistanBackend.Domain.Exceptions;
using FluentValidation;

namespace GeminiAsistanBackend.Api.Middleware;

/// <summary>
/// Translates known exceptions into ProblemDetails-style HTTP responses.
/// </summary>
public sealed class ExceptionHandlingMiddleware
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainValidationException ex)
        {
            _logger.LogWarning(ex, "Domain validation failed.");
            await WriteProblemDetailsAsync(
                context,
                HttpStatusCode.BadRequest,
                ex.Message,
                "domain_validation");
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Request validation failed.");
            var errors = ex.Errors
                .GroupBy(failure => failure.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group
                        .Select(failure => failure.ErrorMessage)
                        .Distinct()
                        .ToArray());

            await WriteProblemDetailsAsync(
                context,
                HttpStatusCode.BadRequest,
                "One or more validation failures have occurred.",
                "request_validation",
                errors);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "An operation validation error occurred.");
            await WriteProblemDetailsAsync(
                context,
                HttpStatusCode.BadRequest, // 400 döner
                ex.Message,                
                "operation_validation");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception.");
            await WriteProblemDetailsAsync(
                context,
                HttpStatusCode.InternalServerError,
                "An unexpected error has occurred.",
                "unhandled_exception");
        }
    }

    private static async Task WriteProblemDetailsAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        string detail,
        string errorType,
        IReadOnlyDictionary<string, string[]>? errors = null)
    {
        if (context.Response.HasStarted)
        {
            return;
        }

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;

        var problemDetails = new
        {
            type = $"https://httpstatuses.io/{(int)statusCode}",
            title = GetTitle(statusCode),
            status = (int)statusCode,
            detail,
            errorType,
            errors
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, SerializerOptions));
    }

    private static string GetTitle(HttpStatusCode statusCode) =>
        statusCode switch
        {
            HttpStatusCode.BadRequest => "Bad Request",
            HttpStatusCode.InternalServerError => "Internal Server Error",
            _ => statusCode.ToString()
        };
}
