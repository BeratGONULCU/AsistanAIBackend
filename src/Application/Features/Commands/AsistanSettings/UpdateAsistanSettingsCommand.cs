using GeminiAsistanBackend.Application.DTOs.AsistanSettings;
using GeminiAsistanBackend.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Commands.AsistanSettings;

public sealed record UpdateAsistanSettingsCommand(
    int id,
    string redmineToken,
    AiProvider activeProvider,
    string? geminiApiKey,
    string? geminiModel,
    string? openAiApikey,
    string? openAiModel,
    string aiFallbackProvider,
    string wakeWord,
    string deadWord,
    string? ollamaModel,
    Boolean voiceInputEnabled
    ) : IRequest<AsistanSettingsResponse>;
