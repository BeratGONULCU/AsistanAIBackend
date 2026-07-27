using GeminiAsistanBackend.Application.DTOs.AsistanSettings;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Commands.AsistanSettings;

public sealed record CreateAssistanSettingsCommand(
    string redmineToken,
    string activeProvider,
    string? geminiApiKey,
    string? geminiModel,
    string? openAiApiKey,
    string? openAiModel,
    string aiFallbackProvider,
    string wakeWord,
    string deadWord,
    string? ollamaModel,
    Boolean voiceInputEnabled
    ) : IRequest<AsistanSettingsResponse>;


