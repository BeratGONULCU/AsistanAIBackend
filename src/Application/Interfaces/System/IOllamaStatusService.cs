namespace GeminiAsistanBackend.Application.Interfaces.System;

public sealed record OllamaStatusResult(bool Available, bool ServiceRunning, IReadOnlyList<string> Models, string Message);

public interface IOllamaStatusService
{
    Task<OllamaStatusResult> CheckAsync(CancellationToken cancellationToken = default);
}
