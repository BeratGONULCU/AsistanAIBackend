using GeminiAsistanBackend.Application;
using GeminiAsistanBackend.Application.Commands;
using GeminiAsistanBackend.Application.DTOs;
using GeminiAsistanBackend.Application.DTOs.CihazKomut;
using GeminiAsistanBackend.Application.DTOs.EgitimDataset;
using GeminiAsistanBackend.Application.DTOs.SesTetikleyici;
using GeminiAsistanBackend.Application.Features.Commands;
using GeminiAsistanBackend.Application.Features.Queries;
using GeminiAsistanBackend.Application.Queries;
using GeminiAsistanBackend.Application.Services;
using GeminiAsistanBackend.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GeminiAsistanBackend.Api.Controllers;

// burada ya da farklı bir controller içerisinde eğitim_dataset excel verilerini egitim.py dosyasına 

[ApiController]
[Route("Api/Egitim_Dataset")]
public sealed class EgitimDatasetController : ControllerBase
{
    public readonly IMediator _mediator;
    public readonly IEgitimDatasetSyncService _syncService;

    private static readonly IReadOnlyDictionary<string, int> labels = new Dictionary<string, int>
    {
        ["question"] = 0,
        ["command"] = 1,
        ["chat"] = 2,
        ["info"] = 3,
        ["uncertain"] = 4
    };

    public EgitimDatasetController(IMediator mediator, IEgitimDatasetSyncService syncService)
    {
        _mediator = mediator;
        _syncService = syncService;
    }

    // burada veri sync için bir istek olacak, bunun içerisinde ;
    // bu tablo controller içerisinde ses_tetikleyicileri.tetikleyici_metin ,tetikleyici_komut içerisinden id değerlerini ,cihaz_komutlari.type içerisinden verileri alacak ve bu yapının içindeki tetikleyici_metin,type_num,tetikleyici_id değerlerini egitim_dataset içerisine ekleyecek.
    // burada karşılaştırma fonksiyonu nerede yazılmalı? Queries içerisinde mi? Service olarak mı?
    // burada GetAllSesTetikleyici yerine başka endpointe erişim nasıl yapılır?

    // şuanda sync edilecek veriler geldi , bu verileri bulk command ile toplu insert olacak.


    [HttpPost("setSync")]
    public async Task<ActionResult> DatasetSync(CancellationToken cancellationToken)
    {
        var resultSesTetikleyici = await _mediator.Send(new GetAllSesTetikleyicileriQuery(),cancellationToken);

        return Ok(resultSesTetikleyici);
    }

    // NOT: Burada sync yapılacak ama yapmadan önce bir kontrol olması gerek. egitim_dataset.sesTetikleyici_id değerleri ile ses_tetikleyicileri.id değerleri birebir aynı mı?
    // üstteki adımda gelen sonuca göre de sync yapılabilir.
    [HttpGet("sync")]
    public async Task<ActionResult> GetSyncData(CancellationToken cancellationToken)
    {
        var SesTetikleyiciMetinler = await _mediator.Send(
            new GetAllSesTetikleyicileriQuery(),
            cancellationToken
        );

        // tetikleyici_komut tablosunda yukarıda gelen id değerleri ile cihaz_komutlari tablosunda arama yapılır.
        var tetikleyiciKomutResult = await _mediator.Send(
            new GetAllTetikleyiciKomutQuery(),
            cancellationToken
            );

        var cihazKomutlariType = await _mediator.Send(
            new GetAllCihazKomutlariQuery(),
            cancellationToken
            );

        if (SesTetikleyiciMetinler is null || !SesTetikleyiciMetinler.Any())
            return NotFound("sestetikleyici içerisinde herhangi bir kayıt bulunamadı.");

        if (tetikleyiciKomutResult is null || !tetikleyiciKomutResult.Any())
            return NotFound("tetikleyicikomut içerisinde herhangi bir kayıt bulunamadı.");

        var sesData = SesTetikleyiciMetinler.Select(x => new
        {
            x.Id,
            x.TetikleyiciMetin,
        }).ToList();

        var tetikleyiciKomutData = tetikleyiciKomutResult.Select(x => new
        {
            x.TetikleticiId,
            x.KomutId,
        }).ToList();

        var cihazKomutlariData = cihazKomutlariType
            .Where(x => x.type != "error")
            .Select(x => new {
            x.Id,
            x.type,
        }).ToList();

        var data =
        from ses in SesTetikleyiciMetinler
        join tk in tetikleyiciKomutResult
            on ses.Id equals tk.TetikleticiId
        join cihaz in cihazKomutlariData
            on tk.KomutId equals cihaz.Id
        select new
        {
            //SesTetikleyiciId = ses.Id,
            ses.TetikleyiciMetin,
            //TetikleyiciKomutId = tk.TetikleticiId,
            //KomutId = tk.KomutId,
            //Type = cihaz.type,
            typeNum = labels.TryGetValue(cihaz.type, out var typeNum) ? typeNum : labels["uncertain"]
        };

        return Ok(data.ToList());
    }

    
     
    [HttpPost("create")]
    public async Task<ActionResult> Create(CancellationToken cancellationToken)
    {
        var SesTetikleyiciMetinler = await _mediator.Send(
            new GetAllSesTetikleyicileriQuery(),
            cancellationToken
        );

        // tetikleyici_komut tablosunda yukarıda gelen id değerleri ile cihaz_komutlari tablosunda arama yapılır.
        var tetikleyiciKomutResult = await _mediator.Send(
            new GetAllTetikleyiciKomutQuery(),
            cancellationToken
            );

        var cihazKomutlariType = await _mediator.Send(
            new GetAllCihazKomutlariQuery(),
            cancellationToken
            );

        if (SesTetikleyiciMetinler is null || !SesTetikleyiciMetinler.Any())
            return NotFound("sestetikleyici içerisinde herhangi bir kayıt bulunamadı.");

        if (tetikleyiciKomutResult is null || !tetikleyiciKomutResult.Any())
            return NotFound("tetikleyicikomut içerisinde herhangi bir kayıt bulunamadı.");

        var sesData = SesTetikleyiciMetinler.Select(x => new
        {
            x.Id,
            x.TetikleyiciMetin,
        }).ToList();

        var tetikleyiciKomutData = tetikleyiciKomutResult.Select(x => new
        {
            x.TetikleticiId,
            x.KomutId,
        }).ToList();

        var cihazKomutlariData = cihazKomutlariType
            .Where(x => x.type != "error")
            .Select(x => new {
                x.Id,
                x.type,
            }).ToList();

        var syncData =
        from ses in SesTetikleyiciMetinler
        join tk in tetikleyiciKomutResult
            on ses.Id equals tk.TetikleticiId
        join cihaz in cihazKomutlariData
            on tk.KomutId equals cihaz.Id
        select new
        {
            //SesTetikleyiciId = ses.Id,
            ses.TetikleyiciMetin,
            //TetikleyiciKomutId = tk.TetikleticiId,
            //KomutId = tk.KomutId,
            //Type = cihaz.type,
            typeNum = labels.TryGetValue(cihaz.type, out var typeNum) ? typeNum : labels["uncertain"],
            ses.Id,
        };

        //var syncData = await _mediator.Send(new GetSyncDataQuery(), cancellationToken);

        if (syncData is null || !syncData.Any())
            return NotFound("Sync edilecek veri bulunamadı.");

        var command = new CreateEgitimDatasetBulkCommand(
            syncData.Select(x => new CreateEgitimDatasetRequest
            {
                TetikleyiciMetin = x.TetikleyiciMetin,
                TypeNum = x.typeNum,
                sesTetikleyiciId = x.Id,
            }).ToList()
        );

        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }
  
    // burada gelen id değerine göre tetikleyici_komut içerisindeki komut_id değerini de alıcaz.
    [HttpGet]
    [ProducesResponseType(typeof(List<SesTetikleyiciResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<EgitimDatasetResponse>>> GetAllSesTetikleyici(CancellationToken cancellationToken)
    {
        var resultSesTetikleyici = await _mediator.Send(new GetAllSesTetikleyicileriQuery(), cancellationToken);

        if(resultSesTetikleyici is null || !resultSesTetikleyici.Any()) // liste var mı ve boş mu
        {
            return NotFound("liste içerisinde herhangi bir bulunamadı.");
        }

        return Ok(resultSesTetikleyici);
    }

    [HttpGet("export")]
    public async Task<IActionResult> ExportExcel(CancellationToken cancellationToken)
    {
        await _syncService.ExportEgitimDatasetToExcelAsync(cancellationToken);
        return Ok("Excel oluşturuldu.");
    }

    [HttpGet("get-excel-path")]
    public async Task<IActionResult> GetExcelPath(CancellationToken cancellationToken)
    {
        var filePath = await _syncService.GetExcelPath(cancellationToken);

        if (string.IsNullOrWhiteSpace(filePath))
            return NotFound("Excel oluşturulamadı.");

        return Ok(filePath);
    }

    [HttpPost("sync")]
    public async Task<IActionResult> Sync(CancellationToken cancellationToken)
    {
        var result = await _syncService.SyncAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("missing")]
    public async Task<IActionResult> GetMissing(CancellationToken cancellationToken)
    {
        var result = await _syncService.GetMissingItemsAsync(cancellationToken);
        return Ok(result);
    }

}
