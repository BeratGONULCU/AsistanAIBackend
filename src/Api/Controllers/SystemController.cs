using GeminiAsistanBackend.Application.Interfaces.System;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Sockets;

namespace GeminiAsistanBackend.Api.Controllers;

[ApiController]
[Route("Api/[controller]")]
public sealed class SystemController(IOllamaStatusService ollamaStatusService) : ControllerBase
{
    private static readonly HttpClient DownloadClient = new();

    [HttpGet("ollama-status")]
    public async Task<ActionResult<OllamaStatusResult>> GetOllamaStatus(CancellationToken cancellationToken)
        => Ok(await ollamaStatusService.CheckAsync(cancellationToken));

    [HttpGet("python-input-status")]
    public async Task<ActionResult<PythonInputStatusResult>> GetPythonInputStatus(
        CancellationToken cancellationToken)
    {
        const string host = "127.0.0.1";
        const int port = 8766;

        try
        {
            using var client = new TcpClient();
            using var timeoutSource =
                CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeoutSource.CancelAfter(TimeSpan.FromSeconds(2));
            await client.ConnectAsync(host, port, timeoutSource.Token);

            return Ok(new PythonInputStatusResult(
                true,
                host,
                port,
                "Python giriş servisi çalışıyor."
            ));
        }
        catch (Exception exception) when (
            exception is SocketException or TimeoutException or OperationCanceledException)
        {
            return Ok(new PythonInputStatusResult(
                false,
                host,
                port,
                "Python giriş servisi çalışmıyor."
            ));
        }
    }

    [HttpGet("check-ollama")]
    public async Task<IActionResult> CheckOllama()
    {
        var processInfo = new ProcessStartInfo
        {
            FileName = "powershell.exe",
            Arguments = "-NoProfile -NonInteractive -Command \"Get-Command ollama -ErrorAction Stop | Select-Object -ExpandProperty Source\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(processInfo);
        if (process is null)
            return StatusCode(500, "PowerShell başlatılamadı.");

        var outputTask = process.StandardOutput.ReadToEndAsync();
        var errorTask = process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();
        var output = await outputTask;
        await errorTask;

        return process.ExitCode != 0
            ? NotFound("Ollama bilgisayarda bulunamadı.")
            : Ok(output.Trim());
    }

    [HttpGet("download-ollama")]
    public async Task<IActionResult> DownloadOllama(CancellationToken cancellationToken)
    {
        const string installerUrl = "https://ollama.com/download/OllamaSetup.exe";
        var response = await DownloadClient.GetAsync(installerUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        if (!response.IsSuccessStatusCode)
            return StatusCode(StatusCodes.Status502BadGateway, "Ollama kurulum dosyası alınamadı.");

        var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        return File(stream, "application/vnd.microsoft.portable-executable", "OllamaSetup.exe");
    }
}

public sealed record PythonInputStatusResult(
    bool Running,
    string Host,
    int Port,
    string Message
);
