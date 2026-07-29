using GeminiAsistanBackend.Domain.Enums;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeminiAsistanBackend.Application.DTOs.AsistanYanit;

public class AsistanSendResponse
{
    public int Id { get; set; }
    public string AsistanYanit { get; set; } = string.Empty;
    public AsistanYanitTuru YanitTuru { get; set; }

    [JsonPropertyName("sessionId")]
    public int SessionId { get; set; }

    public string? RawResponse { get; set; }

    [JsonPropertyName("feedback")]
    public string? Feedback { get; set; }

    public string? KullaniciGeriBildirimi { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int? KomutId { get; set; }
    public JsonElement? JsonData { get; set; }
}