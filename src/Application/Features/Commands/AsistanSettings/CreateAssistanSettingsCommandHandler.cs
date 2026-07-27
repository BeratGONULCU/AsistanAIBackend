using DocumentFormat.OpenXml.Office2010.CustomUI;
using GeminiAsistanBackend.Application.DTOs.AsistanSettings;
using GeminiAsistanBackend.Application.Interfaces;
using GeminiAsistanBackend.Domain.Enums;
using GeminiAsistanBackend.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Commands.AsistanSettings;

public sealed class CreateAssistanSettingsCommandHandler : IRequestHandler<CreateAssistanSettingsCommand ,AsistanSettingsResponse>
{
    private readonly IApplicationDbContext _context;

    public CreateAssistanSettingsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AsistanSettingsResponse> Handle(CreateAssistanSettingsCommand request, CancellationToken cancellationToken)
    {
        var token = request.redmineToken;
        var aiFallbackProvider = request.aiFallbackProvider;
        AiProvider activeProvider;

        // 1. String değeri enum'a dönüştürmeyi dene (Büyük/küçük harf bağımsız)
        if (!Enum.TryParse<AiProvider>(request.activeProvider, ignoreCase: true, out var activeProviderEnum))
        {
            throw new ArgumentException($"Geçersiz AI Sağlayıcısı: '{request.activeProvider}'. Geçerli değerler: {string.Join(", ", Enum.GetNames<AiProvider>())}");
        }

        if (token == null) {
            throw new ArgumentException("redmine token değeri boş olamaz");
        } 

        if (aiFallbackProvider == null)
        {
            throw new ArgumentException("yedek AI token boş geldi. default llama kullanılacak.");

            // burada llama kontrol service çağırılacak.
        }

        // 2. Artık elinde tip güvenli (type-safe) activeProviderEnum var. Switch-expression ile kontrol edebilirsin:
        switch (activeProviderEnum)
        {
            case AiProvider.GEMINI:
                activeProvider = AiProvider.GEMINI;
                // Gemini için apiKey / model null kontrolü yapılabilir
                if (string.IsNullOrWhiteSpace(request.geminiApiKey))
                    throw new InvalidOperationException("Gemini seçildiğinde API Key zorunludur.");
                break;

            case AiProvider.OPENAI:
                activeProvider = AiProvider.OPENAI;
                if (string.IsNullOrWhiteSpace(request.openAiApiKey))
                    throw new InvalidOperationException("OpenAI seçildiğinde API Key zorunludur.");
                break;

            case AiProvider.LLAMA:
                activeProvider = AiProvider.LLAMA;
                // Llama mantığı
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        var entities = new Domain.Entities.AsistanSettings
        {
            redmine_token = token,
            ai_provider = activeProvider,
            gemini_api_key = request.geminiApiKey,
            gemini_model = request.geminiModel,
            openai_model = request.openAiModel,
            ai_fallback_provider = aiFallbackProvider,
            wake_word = request.wakeWord,
            dead_word = request.deadWord,
            ollama_model = request.ollamaModel,
            voice_input_enabled = request.voiceInputEnabled,
};

        await _context.AsistanSettings.AddAsync(entities);
        await _context.SaveChangesAsync(cancellationToken);

        return new AsistanSettingsResponse
        {
            Id = entities.id,
            RedmineToken = token,
            ActiveProvider = activeProvider.ToString(),
            GeminiApiKey = entities.gemini_api_key,
            GeminiModel = entities.gemini_model,
            OpenAiApiKey = entities.openai_api_key,
            OpenAiModel = entities.openai_model,
            AiFallbackProvider = aiFallbackProvider,
            wakeWord = entities.wake_word,
            deadWord = entities.dead_word,
            ollamaModel = entities.ollama_model,
            voiceInputEnabled = entities.voice_input_enabled,
            CreatedAt = entities.created_at,
            UpdatedAt = entities.updated_at,
        };
    }

}
