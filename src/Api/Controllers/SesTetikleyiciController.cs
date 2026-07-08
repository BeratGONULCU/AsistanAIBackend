using GeminiAsistanBackend.Application.DTOs.SesTetikleyici;
using GeminiAsistanBackend.Application.Features.Commands;
using GeminiAsistanBackend.Application.Features.Commands.SesTetikleyiciKomutCommands;
using GeminiAsistanBackend.Application.Features.Queries;
using GeminiAsistanBackend.Application.Features.Queries.SesTetikleyicileriQueries;
using GeminiAsistanBackend.Application.Features.Queries.TetikleyiciKomutQueries;
using GeminiAsistanBackend.Application.Interfaces.SesTetikleyici;
using GeminiAsistanBackend.Application.Models.Todo;
using GeminiAsistanBackend.Application.Services;
using GeminiAsistanBackend.Domain.Enums;
using GeminiAsistanBackend.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GeminiAsistanBackend.Api.Controllers;


[ApiController]
[Route("Api/SesTetikleyici")]
public class SesTetikleyiciController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ISesTetikleyiciService _sesTetikleyiciService;

    public SesTetikleyiciController(IMediator mediator,
        ISesTetikleyiciService sesTetikleyiciService)
    {
        _mediator = mediator;
        _sesTetikleyiciService = sesTetikleyiciService;
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
    public async Task<ActionResult<SesTetikleyiciResponse?>> GetById(int id,CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetSesTetikleyicisiByIdQuery(id),
            cancellationToken);
    }

    [HttpGet("CheckDuplicateMetin")]
    public async Task<ActionResult<bool?>> checkDuplicateSes(string metin, CancellationToken cancellationToken)
    {
        return await _mediator.Send(
            new CheckDuplicateSesTetikleyiciQuery(metin),
            cancellationToken);
    }

    [HttpGet("Get-by-eklenmeturu/{eklenmeturu}")]
    public async Task<IReadOnlyCollection<SesTetikleyiciResponse>> GetSesTetikleyicileriByEklenmeTuru([FromRoute] string eklenmeturu, CancellationToken cancellationToken)
    {
        if (eklenmeturu == null)
            return null;

        if (!Enum.IsDefined(typeof(EklenmeTuru), eklenmeturu))
            throw new ArgumentException("girilen eklenme türü bulunamadı");

        return await _mediator.Send(
            new GetByEklenmeTuruQuery(eklenmeturu),
            cancellationToken);
    }

    [HttpGet("Get-Redmine-Eklenmeturu")]
    public async Task<IReadOnlyCollection<SesTetikleyiciResponse>> GetRedmineSesTetikleyicileri(CancellationToken cancellationToken)
    {
        string eklenmeturu = "REDMINE";
        if (!Enum.IsDefined(typeof(EklenmeTuru), eklenmeturu))
            throw new ArgumentException("eklenme türü hatası");

        return await _mediator.Send(
            new GetByEklenmeTuruQuery(eklenmeturu),
            cancellationToken);
    }

    // Sadece HTTP response için IActionResult 
    [HttpGet("Get-Sestetikleyicileri-bytype")]
    public async Task<IActionResult> GetAllSestetikleyicileriByType([FromQuery] string type,CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetAllSesTetikleyicileriByTypeQuery(type), 
            cancellationToken);

        return Ok(response);
    }

    // hem veri tipi hem HTTP response için ActionResult
    [HttpGet("Count-SesTetikleyici")]
    public async Task<ActionResult<bool>> CountSestetikleyici(CancellationToken cancellationToken)
    {
        return await _sesTetikleyiciService.CountSesTetikleyicileri(cancellationToken);
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

    [HttpPut("Update-{id:int}")]
    public async Task<ActionResult<SesTetikleyiciResponse>> Update(int id,[FromBody] UpdateSesTetikleyiciRequest request, CancellationToken cancellationToken)
    {
        var commandUpdate = new UpdateSesTetikleyiciKomutCommand(
            id,
            request.TetikleyiciMetin,
            request.EklenmeTuru
        );

        var result = await _mediator.Send(commandUpdate, cancellationToken);

        if (result is null)
            return NotFound();
          

        return Ok(result);
    }
}


