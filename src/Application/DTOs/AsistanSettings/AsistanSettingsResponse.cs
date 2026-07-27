using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.DTOs.AsistanSettings;

public class AsistanSettingsResponse
{
    public int Id { get; set; }
    public string RedmineToken { get; set; } = string.Empty;
    public string ActiveProvider { get; set; } = string.Empty;
    public string? GeminiApiKey { get; set; }
    public string? GeminiModel { get; set; }
    public string? OpenAiApiKey { get; set; }
    public string? OpenAiModel { get; set; }
    public string AiFallbackProvider { get; set; } = string.Empty;
    public string wakeWord { get; set; } = string.Empty;
    public string deadWord { get; set; } = string.Empty;
    public Boolean voiceInputEnabled { get; set; } 
    public string? ollamaModel { get; set; } 
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
