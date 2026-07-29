using GeminiAsistanBackend.Application.DTOs.AsistanSettings;
using MediatR;

public sealed record CreateAssistanSettingsCommand(
    string redmineToken,
    string activeProvider,
    string? geminiApiKey,
    string? geminiModel,
    string? openAiApiKey,
    string? openAiModel,
    string? deepseekApiKey,
    string? deepseekModel,
    string? deepseekBaseUrl,
    string aiFallbackProvider,
    string wakeWord,
    string deadWord,
    string? ollamaModel,
    bool voiceInputEnabled
) : IRequest<AsistanSettingsResponse>;