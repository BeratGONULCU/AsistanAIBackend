using GeminiAsistanBackend.Application.DTOs.AsistanSettings;
using GeminiAsistanBackend.Domain.Enums;
using MediatR;

public sealed record UpdateAsistanSettingsCommand(
    int id,
    string redmineToken,
    AiProvider activeProvider,
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