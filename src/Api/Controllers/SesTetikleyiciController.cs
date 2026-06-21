using GeminiAsistanBackend.Application.DTOs.SesTetikleyici;
using GeminiAsistanBackend.Application.Commands;
using GeminiAsistanBackend.Application.Queries;
using GeminiAsistanBackend.Application.Models.Todo;
using GeminiAsistanBackend.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GeminiAsistanBackend.Api.Controllers;


[ApiController]
[Route("Api/SesTetikleyici")]
public class SesTetikleyiciController : ControllerBase
{
    private readonly IMediator _mediator;

    public SesTetikleyiciController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll(CancellationToken cancellationToken)
    {
        //var sesTetikleyiciler = await _sesTetikleyiciService.GetAll(cancellationToken);

        var all = await _mediator.Send(
            new GetAllSesTetikleyicileriQuery(),
            cancellationToken);
        return Ok(all);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SesTetikleyiciResponse>> GetById(int id,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetSesTetikleyicisiByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }

    


    [HttpPost]
    public async Task<ActionResult<SesTetikleyiciResponse>> Create([FromBody] CreateSesTetikleyiciRequest request, CancellationToken cancellationToken )
    {
        var command = new CreateSesTetikleyiciCommand(
            request.TetikleyiciMetin,
            request.EklenmeTuru,
            request.aiConfidenceScore
        );

        var result = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new {id = result.Id},
            result);

    }
}


