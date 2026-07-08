using GeminiAsistanBackend.Application.DTOs.AsistanChat;
using GeminiAsistanBackend.Application.Features.Commands.AsistanYanitCommands;
using GeminiAsistanBackend.Application.Interfaces;
using GeminiAsistanBackend.Application.Interfaces.Python;
using GeminiAsistanBackend.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace GeminiAsistanBackend.Api.Controllers;

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
        if (string.IsNullOrWhiteSpace(request.Message))
        {
            return BadRequest(new
            {
                ok = false,
                message = "Mesaj boş olamaz."
            });
        }

        var sessionId = request.SessionId.GetValueOrDefault();
        if (sessionId <= 0)
        {
            var createdSession = await _mediator.Send(
            new CreateSessionCommand(request.Message, Domain.Enums.AsistanYanitTuru.KOMUT),
            cancellationToken
            );
            
            sessionId = createdSession.SessionID;

            //sessionId = 0;
        }

        var requestId = Guid.NewGuid().ToString("N");

        /*
        var pythonPayload = new
        {
            requestId,
            asistanYanit = request.Message.Trim(),
            yanitTuru = request.asistanYanitTuru,
            sessionId
        };
        */

        var pythonPayload = new
        {
            asistanYanit = request.Message.Trim(),
        };

        string pythonRawResult;
        try
        {
            pythonRawResult = await _pythonService.RunMainScriptAsync(pythonPayload);
        }
        catch (Exception ex)
        {
            return BadRequest(new AsistanChatResponse
            {
                Ok = false,
                SessionId = sessionId,
                UserText = request.Message,
                AssistantResponse = "",
                Message = $"Python çalıştırılırken hata oluştu: {ex.Message}"
            });
        }

        var assistantResponse = ExtractAssistantResponse(pythonRawResult);

        return Ok(new AsistanChatResponse
        {
            Ok = true,
            SessionId = sessionId,
            UserText = request.Message,
            AssistantResponse = assistantResponse,
            Message = "Komut işlendi."
        });
    }

    [HttpPost("checkSession")]
    public async Task<ActionResult<bool>> checkSession()
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


        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();

        process.WaitForExit();

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


        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();

        process.WaitForExit();

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

