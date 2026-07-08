using FluentValidation.AspNetCore;
using GeminiAsistanBackend.Api.Middleware;
using GeminiAsistanBackend.Application.DependencyInjection;
using GeminiAsistanBackend.Application.Interfaces;
using GeminiAsistanBackend.Application.Interfaces.Python;
using GeminiAsistanBackend.Application.Interfaces.RedMineTask;
using GeminiAsistanBackend.Application.Interfaces.SesTetikleyici;
using GeminiAsistanBackend.Application.Services;
using GeminiAsistanBackend.Infrastructure;
using GeminiAsistanBackend.Infrastructure.DependencyInjection;
using GeminiAsistanBackend.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<ISesTetikleyiciService, SesTetikleyiciService>();
builder.Services.AddScoped<IEgitimDatasetSyncService, EgitimDatasetSyncService>();
builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<AppDbContext>());
builder.Services.AddScoped<IPythonService, PythonService>();
builder.Services.AddScoped<IPythonRunService, PythonRunService>();

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        // JSON içindeki "MANUEL" gibi string ifadelerin 
        // arka plandaki EklenmeTuru Enum yapýsýna otomatik dönüþmesini saðlar.
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(entry => entry.Value?.Errors.Count > 0)
                .ToDictionary(
                    entry => entry.Key,
                    entry => entry.Value!.Errors
                        .Select(error => string.IsNullOrWhiteSpace(error.ErrorMessage)
                            ? "Validation failed."
                            : error.ErrorMessage)
                        .Distinct()
                        .ToArray());

            var problemDetails = new
            {
                type = $"https://httpstatuses.io/{StatusCodes.Status400BadRequest}",
                title = "Bad Request",
                status = StatusCodes.Status400BadRequest,
                detail = "One or more validation failures have occurred.",
                errorType = "request_validation",
                errors
            };

            var result = new ObjectResult(problemDetails)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };

            result.ContentTypes.Add("application/problem+json");

            return result;
        };
    });

builder.Services.AddHttpClient("PythonInputServer", client =>
{
    client.BaseAddress = new Uri("http://127.0.0.1:8766/");
    client.Timeout = TimeSpan.FromSeconds(180);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactCors", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<IRedmineService,RedmineService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCors("ReactCors");

app.MapControllers();

app.Run();