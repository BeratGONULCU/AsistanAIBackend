using GeminiAsistanBackend.Application;
using GeminiAsistanBackend.Application.Commands;
using GeminiAsistanBackend.Application.DTOs;
using GeminiAsistanBackend.Application.DTOs.CihazKomut;
using GeminiAsistanBackend.Application.Features.Commands;
using GeminiAsistanBackend.Application.Features.Commands.CihazKomutuCommands;
using GeminiAsistanBackend.Application.Features.Queries;
using GeminiAsistanBackend.Application.Features.Queries.CihazKomutlariQueiries;
using GeminiAsistanBackend.Application.Features.Queries.CihazKomutlariQueries;
using GeminiAsistanBackend.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet("CheckDuplicateCihazKomutlari")]
    public async Task<ActionResult<bool?>> CheckDuplicateCihazKomutlari(string metin,CancellationToken cancellationToken)
    {
        return await _mediator.Send(
            new CheckDuplicateCihazKomutlariQuery(metin),
            cancellationToken);
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

    [HttpPut("Update")]
    public async Task<ActionResult<CihazKomutuResponse>> Update([FromBody] UpdateCihazKomutuRequest request, CancellationToken cancellationToken)
    {
        var updateCommand = new UpdateCihazKomutuCommand(
            request.Id,
            request.Type,
            request.Domain!,
            request.Target!,
            request.Operation!,
            request.CalisacakKod!,
            request.Aciklama!
            );

        var result = await _mediator.Send(updateCommand, cancellationToken);

        return Ok(result);
    }

    [HttpGet("get-by-domain/{domain}")]
    public async Task<ActionResult<List<CihazKomutuResponse>>> GetAllCihazKomutlariByDomain([FromQuery] string domain, CancellationToken cancellationToken)
    {
        return await _mediator.Send(
            new GetAllByDomainQuery(domain),
            cancellationToken);
    }
    
}

