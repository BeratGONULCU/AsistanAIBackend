using DocumentFormat.OpenXml.Spreadsheet;
using GeminiAsistanBackend.Application.DTOs.AsistanSettings;
using GeminiAsistanBackend.Application.Interfaces;
using GeminiAsistanBackend.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Commands.AsistanSettings;

public sealed class UpdateAsistanSettingsCommandHandler : IRequestHandler<UpdateAsistanSettingsCommand, AsistanSettingsResponse>
{
    private readonly IApplicationDbContext _context;

    public UpdateAsistanSettingsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AsistanSettingsResponse> Handle(UpdateAsistanSettingsCommand request, CancellationToken cancellationToken)
    {
        var asistan = await _context.AsistanSettings.FirstOrDefaultAsync(x => x.id == request.id, cancellationToken);

        if (asistan == null)
        {
            throw new Exception($"gönderilen {request.id} id değeri ile ayar bulunamadı");
        }

        asistan.redmine_token = request.redmineToken;
        asistan.ai_provider = request.activeProvider;
        asistan.gemini_api_key = request.geminiApiKey;
        asistan.gemini_model = request.geminiModel;
        asistan.openai_api_key = request.openAiApikey;
        asistan.openai_model = request.openAiModel;
        asistan.ai_fallback_provider = request.aiFallbackProvider;
        asistan.wake_word = request.wakeWord;
        asistan.dead_word = request.deadWord;
        asistan.ollama_model = request.ollamaModel;
        asistan.voice_input_enabled = request.voiceInputEnabled;

        await _context.SaveChangesAsync(cancellationToken);

        return new AsistanSettingsResponse
        {
            Id = asistan.id,
            RedmineToken = asistan.redmine_token,
            ActiveProvider = asistan.ai_provider.ToString(),
            GeminiApiKey = asistan.gemini_api_key,
            GeminiModel = asistan.gemini_model,
            OpenAiApiKey = asistan.openai_api_key,
            OpenAiModel = asistan.openai_model,
            AiFallbackProvider = asistan.ai_fallback_provider,
            CreatedAt = asistan.created_at,
            UpdatedAt = asistan.updated_at,
            wakeWord = asistan.wake_word,
            deadWord = asistan.dead_word,
            ollamaModel = asistan.ollama_model,
            voiceInputEnabled = asistan.voice_input_enabled,
        };
    }

}
