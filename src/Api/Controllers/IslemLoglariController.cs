using GeminiAsistanBackend.Application.Commands;
using GeminiAsistanBackend.Application.DTOs.IslemLog;
using GeminiAsistanBackend.Application.DTOs.SesTetikleyici;
using GeminiAsistanBackend.Application.Features.Commands;
using GeminiAsistanBackend.Application.Features.Commands.IslemLogCommands;
using GeminiAsistanBackend.Application.Features.Queries; 
using GeminiAsistanBackend.Application.Features.Queries.IslemLogQueries;
using GeminiAsistanBackend.Application.Models.Todo;
using GeminiAsistanBackend.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GeminiAsistanBackend.Api.Controllers;



[ApiController]
[Route("Api/IslemLog")]
public class IslemLoglariController : ControllerBase
{
    private readonly IMediator _mediator;

    public IslemLoglariController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // burada GetById yazılacak.

    [HttpGet("{id:int}")]
    public async Task<ActionResult<IslemLogResponse>> GetIslemLogById(int id,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetIslemLogByIdQuery(id), 
            cancellationToken);

        return Ok(result);
    }

    
    [HttpGet("{metin}")]
    public async Task<ActionResult<IslemLogResponse>> GetIslemLogByMetin(string metin,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetIslemLogByMetinQuery(metin),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("Get-By-Durum")]
    public async Task<ActionResult<IslemLogResponse?>> GetIslemLogByDurum(string durum,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetIslemLogByDurumQuery(durum),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("Get-All")]
    public async Task<ActionResult<List<IslemLogResponse>>> GetLogsByDate(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetLogsByDateQuery(),cancellationToken);

        return Ok(result);
    }

    [HttpPost("Create")]
    public async Task<ActionResult<IslemLogResponse>> Create([FromBody] IslemLogRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateIslemLogCommand(
            request.DuyulanSes,
            request.Durum,
            request.CevapMetni,
            request.KomutId,
            request.raw_ai_json
            );

        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }

}
