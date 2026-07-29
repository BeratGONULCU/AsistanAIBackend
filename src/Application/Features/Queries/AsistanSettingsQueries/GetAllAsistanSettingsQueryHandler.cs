using GeminiAsistanBackend.Application.DTOs.AsistanSettings;
using GeminiAsistanBackend.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Queries.AsistanSettingsQueries;

public sealed class GetAllAsistanSettingsQueryHandler : IRequestHandler<GetAllAsistanSettingsQuery, List<AsistanSettingsResponse>>
{
    private readonly IUnitOfWork _unitofwork;

    public GetAllAsistanSettingsQueryHandler(IUnitOfWork unitofwork)
    {
        _unitofwork = unitofwork;
    }

    public async Task<List<AsistanSettingsResponse>> Handle(GetAllAsistanSettingsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _unitofwork.AsistanSettings.GetAllAsync(cancellationToken);

        return entities
            .Select(x => new AsistanSettingsResponse
            {
                Id = x.id,
                RedmineToken = x.redmine_token,
                ActiveProvider = x.ai_provider.ToString(), 
                GeminiApiKey = x.gemini_api_key,
                GeminiModel = x.gemini_model,
                OpenAiApiKey = x.openai_api_key,
                OpenAiModel = x.openai_model,
                AiFallbackProvider = x.ai_fallback_provider,
                DeepseekBaseUrl = x.deepseek_base_url,
                DeepseekModel = x.deepseek_model,
                DeepseekApiKey = x.deepseek_api_key,
                WakeWord = x.wake_word,
                DeadWord = x.dead_word,
                OllamaModel = x.ollama_model, 
                VoiceInputEnabled = x.voice_input_enabled,
                CreatedAt = x.created_at,
                UpdatedAt = x.updated_at,
            }).ToList();
            
    }
}
