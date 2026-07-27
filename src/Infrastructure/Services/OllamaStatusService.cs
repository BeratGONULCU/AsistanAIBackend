using System.Net.Http.Json;
using System.Text.Json.Serialization;
using GeminiAsistanBackend.Application.Interfaces.System;

namespace GeminiAsistanBackend.Infrastructure.Services;

public sealed class OllamaStatusService(HttpClient client) : IOllamaStatusService
{
    public async Task<OllamaStatusResult> CheckAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await client.GetAsync("api/tags", cancellationToken);
            if (!response.IsSuccessStatusCode)
                return new(false, true, [], $"Ollama yanıt verdi ancak durum kodu {(int)response.StatusCode}.");

            var payload = await response.Content.ReadFromJsonAsync<OllamaTagsResponse>(cancellationToken);
            var models = payload?.Models?.Select(x => x.Name).Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(x => x).ToArray() ?? [];

            return new(true, true, models, models.Length > 0
                ? "Ollama çalışıyor ve yerel modeller bulundu."
                : "Ollama çalışıyor ancak yüklü model bulunamadı.");
        }
        catch (HttpRequestException)
        {
            return new(false, false, [], "Ollama bulunamadı veya servis çalışmıyor.");
        }
        catch (TaskCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            return new(false, false, [], "Ollama durum kontrolü zaman aşımına uğradı.");
        }
    }

    private sealed record OllamaTagsResponse([property: JsonPropertyName("models")] OllamaModel[]? Models);
    private sealed record OllamaModel([property: JsonPropertyName("name")] string Name);
}
