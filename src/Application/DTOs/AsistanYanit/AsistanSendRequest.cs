using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiAsistanBackend.Application.DTOs.AsistanYanit;

public class AsistanSendRequest
{
    public string AsistanYanit { get; set; } = null!;
    public string? RawResponse { get; set; }
    public int? KomutId { get; set; }

    [JsonPropertyName("sessionId")]
    public int SessionId { get; set; }

    [JsonPropertyName("feedback")]
    public string? Feedback { get; set; }

    public string? KullaniciGeriBildirimi { get; set; }

    // JSON olarak gelen ek dinamik veriler için
    public JsonElement? JsonData { get; set; }
}