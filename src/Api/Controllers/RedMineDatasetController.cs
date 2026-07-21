using ClosedXML.Excel;
using GeminiAsistanBackend.Application.DTOs.RedMineDto;
using GeminiAsistanBackend.Application.DTOs.RedmineEgitimdataset;
using GeminiAsistanBackend.Application.Features.Commands.RedmineEgitimdatasetCommands;
using GeminiAsistanBackend.Application.Interfaces.RedMineTask;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GeminiAsistanBackend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RedMineDatasetController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IRedmineService _redmineService;

    public RedMineDatasetController(IMediator mediator, IRedmineService redmineService)
    {
        _mediator = mediator;
        _redmineService = redmineService;
    }

    [HttpGet("all/{token}")]
    public async Task<ActionResult<RedMineDataResponse>> GetAllTasks(string token, CancellationToken cancellationToken)
    {
        var response = await _redmineService.GetMyTasksAsync(token, cancellationToken);

        if (response == null)
            return NotFound("Redmine verisi alınamadı");

        return Ok(response);
    }


    [HttpGet("unclosed/{token}")]
    public async Task<ActionResult<RedMineDataResponse>> GetClosedTasks(string token, CancellationToken cancellationToken)
    {
        var response = await _redmineService.GetclosedTasksAsync(token, cancellationToken);

        if (response == null) return NotFound("kapatılan task bulunamadı");

        return Ok(response);
    }

    // burada gönderilen excel verisini stream dönüştürüp json olarak dönüyor.
    // burada response dönmeden nasıl yapılabilir. (kolonlar dinamik veya değişken ise nasıl göndericez?)

    [HttpPost("Excel-read")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult> ReadExcelFile([FromForm] ImportExcelRequest request, CancellationToken cancellationToken)
    {
        // atama için liste tanımladık.
        var items = new List<ReadExcelResponse>();

        // excel dosyasını stream dönüştürüldü
        using var stream = request.File.OpenReadStream();
        using var workbook = new XLWorkbook(stream);

        var worksheet = workbook.Worksheet(1);
        var usedRange = worksheet.RangeUsed();

        if (usedRange == null)
            return BadRequest("Excel dosyası boş");

        var rows = usedRange.RowsUsed().Skip(1); // burada ilk satırdaki veriler kolon kabul edildi

        foreach (var row in rows)
        {
            var item = new ReadExcelResponse
            {
                // GetString ile hücre içerisindeki veriyi aldık
                redmine_tetikleyici_metin = row.Cell(1).GetString(),
                action = row.Cell(2).GetString()
            };
            items.Add(item);
        }

        return Ok(items);
    }


    // Bu endpoint excel verisini db içerisinde ekliyor

    [HttpPost("import-excel")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult> ImportExcel(
    [FromForm] ImportExcelRequest request,
    CancellationToken cancellationToken)
    {
        if (request?.File == null || request.File.Length == 0)
            return BadRequest("Dosya boş.");

        var items = new List<CreateRedmineDatasetRequest>();

        using var stream = request.File.OpenReadStream();
        using var workbook = new XLWorkbook(stream);

        var worksheet = workbook.Worksheet(1);
        var usedRange = worksheet.RangeUsed();

        if (usedRange == null)
            return BadRequest("Excel dosyasında veri bulunamadı.");

        var rows = usedRange.RowsUsed().Skip(1);

        foreach (var row in rows)
        {
            var tetikleyiciMetin = row.Cell(1).GetString();
            var action = row.Cell(2).GetString();

            if (!row.Cell(3).TryGetValue<int>(out var sesTetikleyiciId))
                return BadRequest(
                    $"Geçersiz SesTetikleyiciId. Satır: {row.RowNumber()}");

            if (string.IsNullOrWhiteSpace(tetikleyiciMetin) &&
                string.IsNullOrWhiteSpace(action))
            {
                continue;
            }

            items.Add(new CreateRedmineDatasetRequest
            {
                redmine_tetikleyici_metin = tetikleyiciMetin,
                action = action,
                sesTetikleyici_id = sesTetikleyiciId
            });
        }

        if (items.Count == 0)
            return BadRequest("Excel dosyasında aktarılabilecek kayıt bulunamadı.");

        var results = new List<object?>();

        foreach (var item in items)
        {
            var command = new CreateRedmineEgitimdatasetCommand(
                item.redmine_tetikleyici_metin,
                item.action,
                item.sesTetikleyici_id
            );

            var result = await _mediator.Send(command, cancellationToken);
            results.Add(result);
        }

        return Ok(results);
    }

    [HttpPost("create-item")]
    [ProducesResponseType(typeof(RedmineEgitimdatasetResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateItem(
    [FromBody] CreateRedmineEgitimdatasetRequest request,
    CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (string.IsNullOrWhiteSpace(request.RedmineTetikleyiciMetin))
            return BadRequest("RedmineTetikleyiciMetin boş olamaz.");

        if (string.IsNullOrWhiteSpace(request.Action))
            return BadRequest("Action boş olamaz.");

        if (request.SesTetikleyiciId <= 0)
            return BadRequest("SesTetikleyiciId geçerli olmalıdır.");

        var command = new CreateRedmineEgitimdatasetCommand(
            request.RedmineTetikleyiciMetin,
            request.Action,
            request.SesTetikleyiciId
        );

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }


    [HttpPost("create")]
    public async Task<ActionResult> Create(
    [FromBody] List<CreateRedmineDatasetRequest> request,
    CancellationToken cancellationToken)
    {
        if (request == null || request.Count == 0)
            return BadRequest("Gönderilen veri boş.");

        var results = new List<object?>();

        foreach (var item in request)
        {
            if (string.IsNullOrWhiteSpace(item.redmine_tetikleyici_metin))
                return BadRequest("Redmine tetikleyici metni boş olamaz.");

            if (string.IsNullOrWhiteSpace(item.action))
                return BadRequest("Action boş olamaz.");

            if (item.sesTetikleyici_id <= 0)
                return BadRequest("SesTetikleyiciId geçerli olmalıdır.");

            var command = new CreateRedmineEgitimdatasetCommand(
                item.redmine_tetikleyici_metin,
                item.action,
                item.sesTetikleyici_id
            );

            var result = await _mediator.Send(command, cancellationToken);
            results.Add(result);
        }

        return Ok(results);
    }
}