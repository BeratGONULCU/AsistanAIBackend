using GeminiAsistanBackend.Application;
using GeminiAsistanBackend.Application.Commands;
using GeminiAsistanBackend.Application.DTOs;
using GeminiAsistanBackend.Application.DTOs.CihazKomut;
using GeminiAsistanBackend.Application.DTOs.TetikleyiciKomutlar;
using GeminiAsistanBackend.Application.Features.Commands.TetikleyiciKomutCommands;
using GeminiAsistanBackend.Application.Features.Queries;
using GeminiAsistanBackend.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GeminiAsistanBackend.Api.Controllers;

[ApiController]
[Route("Api/tetikleyici-komut")]
public sealed class tetikleyiciKomutController : ControllerBase
{
    public readonly IMediator _mediator;

    public tetikleyiciKomutController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Create")]
    public async Task<ActionResult<TetikleyiciKomutReponse>> Create([FromBody] CreateTetikleyiciKomutRequest request,CancellationToken cancellationToken)
    {
        var command = new CreateTetikleyiciKomutCommand(
            request.tetikleticiId,
            request.komutId
        );

        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }
}
