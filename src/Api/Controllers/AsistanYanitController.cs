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
using GeminiAsistanBackend.Domain.Enums;
using System.Text.Json;

[ApiController]
[Route("Api/[Controller]")]
public class AsistanYanitController : ControllerBase
{
    private readonly IMediator _mediator;
        
    public AsistanYanitController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // python içerisinden gelecek yanıt için - python gelen her metin için
    [HttpPost("send-asistan-yanit")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AsistanSendResponse>> SendYanit([FromBody] AsistanSendRequest request , CancellationToken cancellationToken)
    {
        // Eğer gelen veri boşluk veya boş string ise null'a çek
        string? temizFeedback = string.IsNullOrWhiteSpace(request.feedback) ? null : request.feedback;
        // nullable
        int? komut_id = request.KomutId <= 0 ? null : request.KomutId;
        AsistanYanitTuru yanitTuru = Domain.Enums.AsistanYanitTuru.YANIT;

        // burada session belirtip göndermek gerek
        var command = new CreateAsistanYanitCommand(
            request.AsistanYanit,
            yanitTuru, // burada default YANIT verecek
            komut_id,
            request.RawResponse, // --> burası db içerisinde yok response da var
            request.SessionId,
            temizFeedback,
            request.JsonData
        );
        
        var result = await _mediator.Send(command,cancellationToken);

        return Ok(result);
    }

    // her girilen komut için - ilk komut hariç
    [HttpPost("send-asistan-komut")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AsistanSendResponse>> SendKomut([FromBody] AsistanSendRequest request, CancellationToken cancellationToken)
    {
        // Eğer gelen veri boşluk veya boş string ise null'a çek
        string? temizFeedback = string.IsNullOrWhiteSpace(request.feedback) ? null : request.feedback;
        // nullable
        int? komut_id = request.KomutId <= 0 ? null : request.KomutId;
        AsistanYanitTuru yanitTuru = Domain.Enums.AsistanYanitTuru.KOMUT;

        // burada session belirtip göndermek gerek
        var command = new CreateAsistanYanitCommand(
            request.AsistanYanit,
            yanitTuru, // burada default YANIT verecek
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
    [HttpPost("send-asistan-onay")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AsistanSendResponse>> SendOnay([FromBody] AsistanSendRequest request, CancellationToken cancellationToken)
    {
        // Eğer gelen veri boşluk veya boş string ise null'a çek
        string? temizFeedback = string.IsNullOrWhiteSpace(request.feedback) ? null : request.feedback;
        // nullable
        int? komut_id = request.KomutId <= 0 ? null : request.KomutId;
        AsistanYanitTuru yanitTuru = Domain.Enums.AsistanYanitTuru.ONAY;

        // burada session belirtip göndermek gerek
        var command = new CreateAsistanYanitCommand(
            request.AsistanYanit,
            yanitTuru, // burada default YANIT verecek
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

        // Eğer gelen veri boşluk veya boş string ise null'a çek
        string? temizFeedback = string.IsNullOrWhiteSpace(request.feedback) ? null : request.feedback;
        // nullable
        int? komut_id = request.KomutId <= 0 ? null : request.KomutId;
        AsistanYanitTuru yanitTuru = Domain.Enums.AsistanYanitTuru.ONAYYANIT;

        // burada session belirtip göndermek gerek
        var command = new CreateAsistanYanitCommand(
            request.AsistanYanit,
            yanitTuru, // burada default YANIT verecek
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


    // girilen komut algılanmadıysa gelecek kısım
    [HttpPost("send-asistan-feedback-error")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AsistanSendResponse>> SendFeedbackError([FromBody] AsistanSendRequest request, CancellationToken cancellationToken)
    {
        // Eğer gelen veri boşluk veya boş string ise null'a çek
        string? temizFeedback = string.IsNullOrWhiteSpace(request.feedback) ? null : request.feedback;
        // nullable
        int? komut_id = request.KomutId <= 0 ? null : request.KomutId;
        AsistanYanitTuru yanitTuru = Domain.Enums.AsistanYanitTuru.FEEDBACKHATA;

        // burada session belirtip göndermek gerek
        var command = new CreateAsistanYanitCommand(
            request.AsistanYanit,
            yanitTuru, // burada default YANIT verecek
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
        [FromBody] UpdateAsistanYanitRequest request, // <-- Yukarıdaki using sayesinde namespace'i kısalttık
        CancellationToken cancellationToken)
    {
        // Yarım kalan noktayı düzelttik ve DTO'dan gelen değeri eşitledik
        AsistanYanitTuru yanitTuru = request.yanitTuru;

        // CQRS/MediatR komutunu oluşturup gönderiyoruz
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
        // Eğer gelen veri boşluk veya boş string ise null'a çek
        string? temizFeedback = string.IsNullOrWhiteSpace(request.feedback) ? null : request.feedback;
        // nullable
        int? komut_id = request.KomutId <= 0 ? null : request.KomutId;
        AsistanYanitTuru yanitTuru = Domain.Enums.AsistanYanitTuru.ACIKLAMA;

        // burada session belirtip göndermek gerek
        var command = new CreateAsistanYanitCommand(
            request.AsistanYanit,
            yanitTuru, // burada default YANIT verecek
            komut_id,
            request.RawResponse,
            request.SessionId,
            temizFeedback,
            request.JsonData
        );

        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    // bu sohbetteki ilk komut için çalışacak - sadece ilk komutta
    [HttpPost("create-sessionID")]
    [ProducesResponseType(StatusCodes.Status200OK, Type =typeof(bool))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AsistanSendResponse>> CreateSessionID([FromBody] createAsistanSessionRequest request,CancellationToken cancellationToken)
    {
        var command = new CreateSessionCommand(
            request.AsistanYanit,
            request.YanitTuru
        );

        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpGet("Get-All")]
    //public async Task<ActionResult<List<EgitimDatasetResponse>>> GetAllAsistanYanit(CancellationToken cancellationToken)
    public async Task<ActionResult<List<AsistanSendResponse>>> GetAllAsistanYanit(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAsistanYanitQuery(), cancellationToken);

        if (result == null || !result.Any())
        {
            return BadRequest("asistan yanıt değerleri getirilemedi");
        }

        return Ok(result);
    }


    // burada sessionID ile getirilecek.
    [HttpGet("Get-BySession-ID")]
    public async Task<ActionResult<List<EgitimDatasetResponse>>> GetBySessionID(int sessionID , CancellationToken cancellationToken)
    {
        var resultSohbet = await _mediator.Send(new GetSohbetBySessionIDQuery(sessionID), cancellationToken);

        if (resultSohbet == null)
        {
            return BadRequest("bu session id ile yanıt yok.");
        }

        return Ok(resultSohbet);
    }

}
