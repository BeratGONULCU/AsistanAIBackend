using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GeminiAsistanBackend.Domain.Enums;


namespace GeminiAsistanBackend.Domain.Entities;

public class AsistanSettings
{
    public int id { get; set; }
    public string redmine_token { get; set; } = string.Empty;
    public AiProvider ai_provider { get; set; } = AiProvider.GEMINI;
    public string? gemini_api_key { get; set; }
    public string? gemini_model { get; set; }
    public string? openai_api_key { get; set; }
    public string? openai_model { get; set; }
    public string ai_fallback_provider { get; set; } = "ollama"; 
    public string wake_word { get; set; }
    public string dead_word { get; set; }
    public bool voice_input_enabled { get; set; } = false;
    public string? ollama_model { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
}
