namespace GeminiAsistanBackend.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using MediatR;
using GeminiAsistanBackend.Application.Features.Commands.AsistanYanitCommands;
using Microsoft.AspNetCore.Http;
using GeminiAsistanBackend.Application.DTOs.AsistanYanit;
using GeminiAsistanBackend.Application.DTOs.EgitimDataset;
using GeminiAsistanBackend.Application.Features.Queries.AsistanYanitQueries;
using GeminiAsistanBackend.Application.Features.Commands.EgitimDataSet;
using GeminiAsistanBackend.Application.Features.Commands.RedmineEgitimdatasetCommands;
using GeminiAsistanBackend.Application.Features.Commands.SesTetikleyiciKomutCommands;
using GeminiAsistanBackend.Infrastructure;
using Microsoft.EntityFrameworkCore;
using GeminiAsistanBackend.Domain.Enums;
using System.Text.Json;

[ApiController]
[Route("Api/[Controller]")]
public class AsistanYanitController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly AppDbContext _dbContext;

    public AsistanYanitController(
        IMediator mediator,
        AppDbContext dbContext)
    {
        _mediator = mediator;
        _dbContext = dbContext;
    }

    // python içerisinden gelecek yanıt için - python gelen her metin için
    [HttpPost("send-asistan-yanit")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AsistanSendResponse>> SendYanit([FromBody] AsistanSendRequest request, CancellationToken cancellationToken)
    {
        // Eğer gelen veri boşluk veya boş string ise null'a çek
        string? temizFeedback = string.IsNullOrWhiteSpace(request.Feedback) ? null : request.Feedback;
        int? komut_id = request.KomutId <= 0 ? null : request.KomutId;
        AsistanYanitTuru yanitTuru = Domain.Enums.AsistanYanitTuru.YANIT;

        var command = new CreateAsistanYanitCommand(
            request.AsistanYanit,
            yanitTuru,
            komut_id,
            request.RawResponse,
            request.SessionId,
            temizFeedback,
            request.JsonData
        );

        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    // her girilen komut için - ilk komut hariç
    [HttpPost("send-asistan-komut")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AsistanSendResponse>> SendKomut([FromBody] AsistanSendRequest request, CancellationToken cancellationToken)
    {
        string? temizFeedback = string.IsNullOrWhiteSpace(request.Feedback) ? null : request.Feedback;
        int? komut_id = request.KomutId <= 0 ? null : request.KomutId;
        AsistanYanitTuru yanitTuru = Domain.Enums.AsistanYanitTuru.KOMUT;

        var command = new CreateAsistanYanitCommand(
            request.AsistanYanit,
            yanitTuru,
            komut_id,
            request.RawResponse,
            request.SessionId,
            temizFeedback,
            request.JsonData
        );

        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpPost("send-asistan-onay")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AsistanSendResponse>> SendOnay([FromBody] AsistanSendRequest request, CancellationToken cancellationToken)
    {
        string? temizFeedback = string.IsNullOrWhiteSpace(request.Feedback) ? null : request.Feedback;
        int? komut_id = request.KomutId <= 0 ? null : request.KomutId;
        AsistanYanitTuru yanitTuru = Domain.Enums.AsistanYanitTuru.ONAY;

        var command = new CreateAsistanYanitCommand(
            request.AsistanYanit,
            yanitTuru,
            komut_id,
            request.RawResponse,
            request.SessionId,
            temizFeedback,
            request.JsonData
        );

        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpPost("send-asistan-onayYanit")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AsistanSendResponse>> SendOnayYanit([FromBody] AsistanSendRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.AsistanYanit))
            return BadRequest("Onay yanıtı boş olamaz.");

        var normalizedAnswer = request.AsistanYanit.Trim().ToLowerInvariant();
        if (normalizedAnswer is not ("evet" or "hayır" or "hayir"))
            return BadRequest("Onay yanıtı yalnızca 'evet' veya 'hayır' olabilir.");

        string? temizFeedback = string.IsNullOrWhiteSpace(request.Feedback) ? null : request.Feedback;
        int? komut_id = request.KomutId <= 0 ? null : request.KomutId;
        AsistanYanitTuru yanitTuru = Domain.Enums.AsistanYanitTuru.ONAYYANIT;

        var command = new CreateAsistanYanitCommand(
            request.AsistanYanit,
            yanitTuru,
            komut_id,
            request.RawResponse,
            request.SessionId,
            temizFeedback,
            request.JsonData
        );

        var result = await _mediator.Send(command, cancellationToken);

        if (normalizedAnswer == "evet")
        {
            var validationError = await SaveApprovedTrainingData(
                request.JsonData,
                cancellationToken
            );

            if (validationError is not null)
                return BadRequest(validationError);
        }

        return Ok(result);
    }

    private async Task<string?> SaveApprovedTrainingData(
        JsonElement? jsonData,
        CancellationToken cancellationToken)
    {
        if (jsonData is null || jsonData.Value.ValueKind != JsonValueKind.Object)
            return "Onaylanan yanıtın JsonData bilgisi bulunamadı.";

        var json = jsonData.Value;
        var originalText = GetJsonString(json, "originalText");
        var responseType = GetJsonString(json, "type")?.ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(originalText))
            return "JsonData.originalText bilgisi bulunamadı.";

        if (string.IsNullOrWhiteSpace(responseType))
            return "JsonData.type bilgisi bulunamadı.";

        var confidence = GetJsonDouble(json, "confidence");
        var isCommand = responseType == "command";
        var trigger = await _mediator.Send(
            new CreateSesTetikleyiciCommand(
                originalText,
                isCommand ? EklenmeTuru.REDMINE : EklenmeTuru.AI_LEARNED,
                confidence
            ),
            cancellationToken
        );

        if (isCommand)
        {
            var action =
                GetJsonString(json, "calisacakKod") ??
                GetJsonString(json, "operation");

            if (string.IsNullOrWhiteSpace(action))
                return "Command onayı için JsonData.calisacakKod veya operation bilgisi bulunamadı.";

            await _mediator.Send(
                new CreateRedmineEgitimdatasetCommand(
                    originalText,
                    action,
                    trigger.Id
                ),
                cancellationToken
            );
        }
        else
        {
            var typeNumber = responseType switch
            {
                "question" => 0,
                "chat" => 2,
                "info" => 3,
                _ => 4
            };

            await _mediator.Send(
                new CreateEgitimDatasetCommand(
                    originalText,
                    typeNumber,
                    trigger.Id
                ),
                cancellationToken
            );
        }

        return null;
    }

    private static string? GetJsonString(JsonElement json, string propertyName)
    {
        return json.TryGetProperty(propertyName, out var property) &&
               property.ValueKind == JsonValueKind.String
            ? property.GetString()
            : null;
    }

    private static double? GetJsonDouble(JsonElement json, string propertyName)
    {
        return json.TryGetProperty(propertyName, out var property) &&
               property.TryGetDouble(out var value)
            ? value
            : null;
    }

    [HttpPost("delete-by-sessionID")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<bool>> DeleteBySessionID([FromBody] DeleteSessionCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    // girilen komut algılanmadıysa gelecek kısım
    [HttpPost("send-asistan-feedback-error")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AsistanSendResponse>> SendFeedbackError([FromBody] AsistanSendRequest request, CancellationToken cancellationToken)
    {
        string? temizFeedback = string.IsNullOrWhiteSpace(request.Feedback) ? null : request.Feedback;
        int? komut_id = request.KomutId <= 0 ? null : request.KomutId;
        AsistanYanitTuru yanitTuru = Domain.Enums.AsistanYanitTuru.FEEDBACKHATA;

        var command = new CreateAsistanYanitCommand(
            request.AsistanYanit,
            yanitTuru,
            komut_id,
            request.RawResponse,
            request.SessionId,
            temizFeedback,
            request.JsonData
        );

        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpPatch("update-asistanyanit/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AsistanSendResponse>> UpdateYanit(
        [FromRoute] int id,
        [FromBody] UpdateAsistanYanitRequest request,
        CancellationToken cancellationToken)
    {
        AsistanYanitTuru yanitTuru = request.yanitTuru;

        var command = new UpdateAsistanYanitCommand(id, yanitTuru);
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    // python içerisinden gelecek yanıt için - python gelen her metin için
    [HttpPost("send-asistan-aciklama")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AsistanSendResponse>> SendAciklama([FromBody] AsistanSendRequest request, CancellationToken cancellationToken)
    {
        string? temizFeedback = string.IsNullOrWhiteSpace(request.Feedback) ? null : request.Feedback;
        int? komut_id = request.KomutId <= 0 ? null : request.KomutId;
        AsistanYanitTuru yanitTuru = Domain.Enums.AsistanYanitTuru.ACIKLAMA;

        var command = new CreateAsistanYanitCommand(
            request.AsistanYanit,
            yanitTuru,
            komut_id,
            request.RawResponse,
            request.SessionId,
            temizFeedback,
            request.JsonData
        );

        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpPatch("archive-session/{sessionId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ArchiveSession(
    [FromRoute] int sessionId,
    CancellationToken cancellationToken)
    {
        if (sessionId <= 0)
        {
            return BadRequest(new
            {
                ok = false,
                message = "Geçerli bir session ID gönderilmelidir."
            });
        }

        var affectedRows = await _dbContext.AsistanYanit
            .Where(x => x.SessionId == sessionId)
            .ExecuteUpdateAsync(
                setters => setters
                    .SetProperty(x => x.IsArchived, true)
                    .SetProperty(x => x.updated_at, DateTime.UtcNow),
                cancellationToken
            );

        if (affectedRows == 0)
        {
            return NotFound(new
            {
                ok = false,
                sessionId,
                message = "Bu session ID ile sohbet bulunamadı."
            });
        }

        return Ok(new
        {
            ok = true,
            sessionId,
            affectedRows,
            message = "Sohbet arşivlendi."
        });
    }

    [HttpPatch("unarchive-session/{sessionId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UnarchiveSession(
    [FromRoute] int sessionId,
    CancellationToken cancellationToken)
    {
        if (sessionId <= 0)
        {
            return BadRequest(new
            {
                ok = false,
                message = "Geçerli bir session ID gönderilmelidir."
            });
        }

        var affectedRows = await _dbContext.AsistanYanit
            .Where(x =>
                x.SessionId == sessionId &&
                x.IsArchived)
            .ExecuteUpdateAsync(
                setters => setters
                    .SetProperty(x => x.IsArchived, false)
                    .SetProperty(x => x.updated_at, DateTime.UtcNow),
                cancellationToken
            );

        if (affectedRows == 0)
        {
            return NotFound(new
            {
                ok = false,
                sessionId,
                message =
                    "Arşivlenmiş sohbet bulunamadı."
            });
        }

        return Ok(new
        {
            ok = true,
            sessionId,
            affectedRows,
            message = "Sohbet arşivden kaldırıldı."
        });
    }

    [HttpPost("create-sessionID")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AsistanSendResponse>> CreateSessionID([FromBody] createAsistanSessionRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateSessionCommand(
            request.AsistanYanit,
            request.YanitTuru
        );

        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpGet("Get-All")]
    public async Task<ActionResult<List<AsistanSendResponse>>> GetAllAsistanYanit(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAsistanYanitQuery(), cancellationToken);

        if (result == null || !result.Any())
        {
            return BadRequest("asistan yanıt değerleri getirilemedi");
        }

        return Ok(result);
    }

    [HttpGet("Get-BySession-ID")]
    public async Task<ActionResult<List<EgitimDatasetResponse>>> GetBySessionID(int sessionID, CancellationToken cancellationToken)
    {
        var resultSohbet = await _mediator.Send(new GetSohbetBySessionIDQuery(sessionID), cancellationToken);

        if (resultSohbet == null)
        {
            return BadRequest("bu session id ile yanıt yok.");
        }

        return Ok(resultSohbet);
    }

    [HttpGet("Get-Archived-Sohbet")]
    public async Task<ActionResult<List<AsistanSendResponse>>> GetArchivedSohbetler(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetArchivedYanitQuery(), cancellationToken);

        if (result == null)
        {
            return BadRequest("arşivlenmiş sohbet yoktur");
        }

        return Ok(result);
    }
}