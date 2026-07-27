using GeminiAsistanBackend.Application.DTOs.AsistanChat;
using GeminiAsistanBackend.Application.DTOs.AsistanSettings;
using GeminiAsistanBackend.Application.Features.Commands.AsistanSettings;
using GeminiAsistanBackend.Application.Features.Commands.AsistanYanitCommands;
using GeminiAsistanBackend.Application.Features.Queries.AsistanSettingsQueries;
using GeminiAsistanBackend.Application.Interfaces;
using GeminiAsistanBackend.Application.Interfaces.Python;
using GeminiAsistanBackend.Domain.Entities;
using GeminiAsistanBackend.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace GeminiAsistanBackend.Api.Controllers;


[ApiController]
[Route("Api/[controller]")]
public class AssistanSettingsController : ControllerBase
{
    public readonly IMediator _mediator;
    public readonly IApplicationDbContext _context;

    public AssistanSettingsController(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(AsistanSettingsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AsistanSettingsResponse>> Create([FromBody] AsistanSettingsRequest request, CancellationToken cancellationToken)
    {
        // 1. Mevcut kayıt var mı kontrol et
        var existingSettings = await _mediator.Send(new GetAllAsistanSettingsQuery(), cancellationToken);

        // Eğer veritabanında zaten bir kayıt varsa UPDATE işlemini tetikle
        var firstSetting = existingSettings?.FirstOrDefault(); // ya da listenedeki kayıt

        // Eğer request.activeProvider geçerli bir Enum ismi ise onu alır, 
        // değilse otomatik olarak Enum'ın ilk/varsayılan (0) değerini dinamik olarak atar.
        if (!Enum.TryParse<AiProvider>(request.activeProvider, ignoreCase: true, out var activeProviderEnum))
        {
            activeProviderEnum = default(AiProvider);
        }

        if (firstSetting != null)
        {
            var updateCommand = new UpdateAsistanSettingsCommand(
                firstSetting.Id, // Var olan kaydın ID'si
                request.redmineToken,
                activeProviderEnum,
                request.geminiApiKey,
                request.geminiModel,
                request.openAiApiKey,
                request.openAiModel,
                request.aiFallbackProvider,
                request.wakeWord,
                request.deadWord,
                request.ollamaModel,
                request.voiceInputEnabled
            );

            var updateResult = await _mediator.Send(updateCommand, cancellationToken);
            return Ok(updateResult); // Güncellenmiş veriyi döndür
        }

        // 2. Kayıt yoksa CREATE işlemini yap
        var createCommand = new CreateAssistanSettingsCommand(
            request.redmineToken,
            request.activeProvider,
            request.geminiApiKey,
            request.geminiModel,
            request.openAiApiKey,
            request.openAiModel,
            request.aiFallbackProvider,
            request.wakeWord,
            request.deadWord,
            request.ollamaModel,
            request.voiceInputEnabled
        );

        var createResult = await _mediator.Send(createCommand, cancellationToken);
        return Ok(createResult);
    }

    [HttpGet("Get-All")]
    [ProducesResponseType(typeof(AsistanSettingsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<AsistanSettingsResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllAsistanSettingsQuery(), cancellationToken);

        if(result == null || !result.Any())
        {
            return BadRequest("konfigürasyon ayarları bulunamadı");
        }

        return Ok(result);
    }

    [HttpPut("update")] 
    public async Task<ActionResult> Update([FromBody] UpdateAsistanSettingsCommand request,CancellationToken cancellationToken)
    {
        var updateResult = await _mediator.Send(request, cancellationToken);

        if (updateResult == null)
        {
            return BadRequest("güncelleme işlemi başarısız");
        }

        return Ok(updateResult);
    }

}
