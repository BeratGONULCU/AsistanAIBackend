namespace GeminiAsistanBackend.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using GeminiAsistanBackend.Application.Features.Commands.AsistanYanitCommands;
using Microsoft.AspNetCore.Http;
using GeminiAsistanBackend.Application.DTOs.AsistanYanit;
using GeminiAsistanBackend.Application.DTOs.EgitimDataset;
using GeminiAsistanBackend.Application.Features.Queries.AsistanYanitQueries;
using GeminiAsistanBackend.Domain.Enums;

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
