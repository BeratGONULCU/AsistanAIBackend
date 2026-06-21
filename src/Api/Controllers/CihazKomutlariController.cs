using MediatR;
using GeminiAsistanBackend.Application;
using GeminiAsistanBackend.Application.DTOs;
using GeminiAsistanBackend.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using GeminiAsistanBackend.Application.DTOs.CihazKomut;
using GeminiAsistanBackend.Application.Queries;
using GeminiAsistanBackend.Application.Commands;

namespace GeminiAsistanBackend.Api.Controllers;

[ApiController]
[Route("Api/cihaz_komutlari")]
public sealed class CihazKomutlariController : ControllerBase
{
    /*
     * controller görevi şuanda sadece bu
     * 
       Request al
       Command/Query oluştur
       Mediator'a gönder
       HTTP response dön
     */
    private readonly IMediator _mediator;

    public CihazKomutlariController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<CihazKomutuResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetAllCihazKomutlariQuery(),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CihazKomutuResponse>> GetById(int id,CancellationToken cancellationToken)
    {
        var resultId = await _mediator.Send(
            new GetCihazKomutuByIdQuery(id),
            cancellationToken
            );

        if (resultId is null)
            return NotFound();

        return Ok(resultId);
    }

    // Burada cihaz_komutlari tablosunda calisacak_kod ve diğer kolon değerleri için AnyAsync içerisinde arama yapacak. 

    [HttpPost]
    public async Task<ActionResult<CihazKomutuResponse>> Create([FromBody] CreateCihazKomutuRequest request,CancellationToken cancellationToken)
    {
        var command = new CreateCihazKomutuCommand(
            request.Type,
            request.Domain!,
            request.Target!,
            request.Operation,
            request.CalisacakKod!,
            request.Aciklama!
            );

        var result = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
    }
}

