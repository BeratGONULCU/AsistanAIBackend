using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeminiAsistanBackend.Domain.Enums;

namespace GeminiAsistanBackend.Application.DTOs.AsistanSettings;

public class UpdateAsistanSettingsRequest
{
    public int Id { get; set; }
    public string RedmineToken { get; set; } = string.Empty;
    public AiProvider ActiveProvider { get; set; }
    public string? GeminiApiKey { get; set; }
    public string? GeminiModel { get; set; }
    public string? OpenAiApiKey { get; set; }
    public string? OpenAiModel { get; set; }
    public string AiFallbackProvider { get; set; } = string.Empty;
    public string wakeWord { get; set; } = string.Empty;
    public string deadWord { get; set; } = string.Empty;
    public string? ollamaModel { get; set; } 
}