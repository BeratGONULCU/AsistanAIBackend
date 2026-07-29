namespace GeminiAsistanBackend.Api.Controllers;

using GeminiAsistanBackend.Application.DTOs.AsistanChat;
using GeminiAsistanBackend.Application.Features.Commands.AsistanYanitCommands;
using GeminiAsistanBackend.Application.Interfaces;
using GeminiAsistanBackend.Application.Interfaces.Python;
using GeminiAsistanBackend.Domain.Entities;
using GeminiAsistanBackend.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

[ApiController]
[Route("Api/[controller]")]
public class AsistanChatController : ControllerBase
{
    private readonly IApplicationDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IPythonService _pythonService;
    private readonly IMediator _mediator;

    public AsistanChatController(
        IApplicationDbContext context,
        IHttpClientFactory httpClientFactory,
        IMediator mediator,
        IPythonService pythonService)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
        _pythonService = pythonService;
        _mediator = mediator;
    }

    [HttpPost("send")]
    public async Task<ActionResult<AsistanChatResponse>> Send(
    [FromBody] AsistanChatRequest request,
    CancellationToken cancellationToken)
    {
        if (request == null)
        {
            return BadRequest(new AsistanChatResponse
            {
                Ok = false,
                Message = "Request boş olamaz."
            });
        }

        if (string.IsNullOrWhiteSpace(request.Message))
        {
            return BadRequest(new AsistanChatResponse
            {
                Ok = false,
                Message = "Mesaj boş olamaz."
            });
        }

        var message = request.Message.Trim();
        var sessionId = request.SessionId.GetValueOrDefault();

        var yanitTuru = request.asistanYanitTuru;

        try
        {
            if (sessionId <= 0)
            {
                var createdSession = await _mediator.Send(
                    new CreateSessionCommand(
                        message,
                        yanitTuru
                    ),
                    cancellationToken
                );

                // CS1061 DÜZELTMESİ: SessionID -> SessionId yapıldı
                sessionId = createdSession.SessionId;

                if (sessionId <= 0)
                {
                    throw new InvalidOperationException(
                        "Session oluşturulamadı."
                    );
                }
            }
            else
            {
                var sessionExists = await _context.AsistanYanit
                    .AnyAsync(
                        x => x.SessionId == sessionId,
                        cancellationToken
                    );

                if (!sessionExists)
                {
                    return NotFound(new AsistanChatResponse
                    {
                        Ok = false,
                        SessionId = sessionId,
                        UserText = message,
                        AssistantResponse = "",
                        Message = $"Session bulunamadı: {sessionId}"
                    });
                }

                if (yanitTuru != AsistanYanitTuru.ONAYYANIT)
                {
                    var command = new CreateAsistanYanitCommand(
                        message,
                        yanitTuru,
                        null,
                        null,
                        sessionId,
                        null,
                        null
                    );

                    await _mediator.Send(
                        command,
                        cancellationToken
                    );
                }
            }
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new AsistanChatResponse
                {
                    Ok = false,
                    SessionId = sessionId,
                    UserText = message,
                    AssistantResponse = "",
                    Message = $"Mesaj kaydedilirken hata oluştu: {ex.Message}"
                }
            );
        }

        var requestId = Guid.NewGuid().ToString("N");

        var pythonPayload = new
        {
            requestId,
            asistanYanit = message,
            yanitTuru = yanitTuru.ToString(),
            sessionId,
            saveToBackend = false
        };

        string pythonRawResult;

        try
        {
            pythonRawResult = await _pythonService.RunMainScriptAsync(
                pythonPayload
            );
        }
        catch (Exception ex)
        {
            return BadRequest(new AsistanChatResponse
            {
                Ok = false,
                SessionId = sessionId,
                UserText = message,
                AssistantResponse = "",
                Message = $"Python çalıştırılırken hata oluştu: {ex.Message}"
            });
        }

        var assistantResponse = ExtractAssistantResponse(
            pythonRawResult
        );

        return Ok(new AsistanChatResponse
        {
            Ok = true,
            SessionId = sessionId,
            UserText = message,
            AssistantResponse = assistantResponse,
            Message = "Komut işlendi."
        });
    }

    [HttpPost("checkSession")]
    public async Task<ActionResult<bool>> checkSession()
    {
        var script =
            @"$connections = Get-NetTCPConnection -LocalPort 8766 -State Listen -ErrorAction SilentlyContinue; " +
            @"if (-not $connections) { Write-Output 'False' }  else { Write-Output 'True' }";

        var psi = new ProcessStartInfo
        {
            FileName = "powershell",
            Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{script}\"",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        using var process = Process.Start(psi);

        if (process == null)
            return StatusCode(500, "PowerShell başlatılamadı.");

        var output = await process.StandardOutput.ReadToEndAsync();
        var error = await process.StandardError.ReadToEndAsync();

        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
            return StatusCode(500, error);

        if (output.Contains("NO_PROCESS"))
        {
            return false;
        }

        return true;
    }

    [HttpPost("cancelSession")]
    public async Task<IActionResult> cancelSession()
    {
        var script =
            "$connections = Get-NetTCPConnection -LocalPort 8766 -State Listen -ErrorAction SilentlyContinue; " +
            "if (-not $connections) { Write-Output 'NO_PROCESS'; exit 0 }; " +
            "$pids = $connections.OwningProcess | Sort-Object -Unique; " +
            "$pids | ForEach-Object { Stop-Process -Id $_ -Force }; " +
            "$pids";

        var psi = new ProcessStartInfo
        {
            FileName = "powershell",
            Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{script}\"",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        using var process = Process.Start(psi);

        if (process == null)
            return StatusCode(500, "PowerShell başlatılamadı.");

        var output = await process.StandardOutput.ReadToEndAsync();
        var error = await process.StandardError.ReadToEndAsync();

        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
            return StatusCode(500, error);

        if (output.Contains("NO_PROCESS"))
        {
            return Ok(new
            {
                ok = true,
                message = "8766 portunda çalışan process bulunamadı.",
                killedPids = Array.Empty<int>()
            });
        }

        return Ok(new
        {
            ok = true,
            message = "8766 portundaki process kapatıldı.",
            killedPids = output
        });
    }

    private static string ExtractAssistantResponse(string rawPythonResult)
    {
        if (string.IsNullOrWhiteSpace(rawPythonResult))
            return "";

        try
        {
            var result = JsonSerializer.Deserialize<PythonInputResponse>(
                rawPythonResult,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            return result?.AssistantResponse
                   ?? result?.AsistanYanit
                   ?? result?.Output
                   ?? result?.Message
                   ?? rawPythonResult;
        }
        catch
        {
            return rawPythonResult;
        }
    }
}