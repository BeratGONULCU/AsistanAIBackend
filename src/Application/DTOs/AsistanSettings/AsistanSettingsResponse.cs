public class AsistanSettingsResponse
{
    public int Id { get; set; }
    public string RedmineToken { get; set; } = string.Empty;
    public string ActiveProvider { get; set; } = string.Empty;

    public string? GeminiApiKey { get; set; }
    public string? GeminiModel { get; set; }

    public string? OpenAiApiKey { get; set; }
    public string? OpenAiModel { get; set; }

    public string? DeepseekApiKey { get; set; }
    public string? DeepseekModel { get; set; }
    public string? DeepseekBaseUrl { get; set; }

    public string AiFallbackProvider { get; set; } = string.Empty;
    public string WakeWord { get; set; } = string.Empty;
    public string DeadWord { get; set; } = string.Empty;
    public bool VoiceInputEnabled { get; set; }
    public string? OllamaModel { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}