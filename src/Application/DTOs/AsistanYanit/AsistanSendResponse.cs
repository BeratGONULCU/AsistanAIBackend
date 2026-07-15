using GeminiAsistanBackend.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.DTOs.AsistanYanit;

public class AsistanSendResponse
{
    public int Id { get; set; }
    public string AsistanYanit { get; set; } = string.Empty;
    public AsistanYanitTuru YanitTuru { get; set; }
    [JsonPropertyName("sessionID")]
    public int SessionID { get; set; }
    public string? RawResponse { get; set; }
    public string? feedback { get; set; }
    public DateTime CreatedAt { get; set; } 
    public DateTime UpdatedAt { get; set; }
    public int? KomutId { get; set; }
}
