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
            request.SessionId,
            temizFeedback
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
            request.SessionId,
            temizFeedback
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
    public async Task<ActionResult<List<EgitimDatasetResponse>>> GetAllAsistanYanit(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAsistanYanitQuery(), cancellationToken);

        if (result == null || !result.Any())
        {
            return BadRequest("asistan yanıt değerleri getirilemedi");
        }

        return Ok(result);
    }

}
