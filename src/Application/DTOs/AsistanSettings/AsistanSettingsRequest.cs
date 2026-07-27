using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.DTOs.AsistanSettings;

public class AsistanSettingsRequest
{
    public string redmineToken {  get; set; } 
    public string activeProvider { get; set; }
    public string? geminiApiKey { get; set; }
    public string? geminiModel { get; set; }
    public string? openAiApiKey { get; set; }
    public string? openAiModel { get; set; }
    public string aiFallbackProvider { get; set; } = "ollama";
    public string wakeWord { get; set; } = "asistan";
    public string deadWord { get; set; } = "kapat";
    public string ollamaModel { get; set; } = "llama3.1:8b";
    public Boolean voiceInputEnabled { get; set; }
}