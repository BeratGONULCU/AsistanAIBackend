using GeminiAsistanBackend.Application.Commands;
using GeminiAsistanBackend.Application.DTOs.EgitimDataset;
using GeminiAsistanBackend.Application.Features.Commands;
using GeminiAsistanBackend.Application.Features.Queries;
using GeminiAsistanBackend.Application.Queries;
using ClosedXML.Excel; 
using MediatR;

namespace GeminiAsistanBackend.Application.Services;

public sealed class EgitimDatasetSyncService : IEgitimDatasetSyncService
{
    private readonly IMediator _mediator;
    private static readonly IReadOnlyDictionary<string, int> Labels = new Dictionary<string, int>
    {
        ["question"] = 0,
        ["command"] = 1,
        ["chat"] = 2,
        ["info"] = 3,
        ["uncertain"] = 4
    };

    public EgitimDatasetSyncService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<List<CreateEgitimDatasetRequest>> GetMissingItemsAsync(CancellationToken cancellationToken)
    {
        var sesTetikleyiciler = await _mediator.Send(new GetAllSesTetikleyicileriQuery(), cancellationToken);
        var egitimDataset = await _mediator.Send(new GetAllEgitimDatasetQuery(), cancellationToken);
        var tetikleyiciKomutlar = await _mediator.Send(new GetAllTetikleyiciKomutQuery(), cancellationToken);
        var cihazKomutlari = await _mediator.Send(new GetAllCihazKomutlariQuery(), cancellationToken);

        if (sesTetikleyiciler is null || egitimDataset is null || tetikleyiciKomutlar is null || cihazKomutlari is null)
            return new List<CreateEgitimDatasetRequest>();

        var mevcutSesIdleri = egitimDataset
            .Select(x => x.SesTetikleyiciId)
            .ToHashSet();

        var cihazKomutlariData = cihazKomutlari
            .Where(x => !string.Equals(x.type, "error", StringComparison.OrdinalIgnoreCase))
            .ToList();

        var syncData =
            from ses in sesTetikleyiciler
            join tk in tetikleyiciKomutlar
                on ses.Id equals tk.TetikleticiId
            join cihaz in cihazKomutlariData
                on tk.KomutId equals cihaz.Id
            where !mevcutSesIdleri.Contains(ses.Id)
            select new CreateEgitimDatasetRequest
            {
                sesTetikleyiciId = ses.Id,
                TetikleyiciMetin = ses.TetikleyiciMetin,
                TypeNum = Labels.TryGetValue(cihaz.type, out var typeNum) ? typeNum : Labels["uncertain"]
            };

        return syncData.ToList();
    }

    public async Task<List<EgitimDatasetResponse>> SyncAsync(CancellationToken cancellationToken)
    {
        var missingItems = await GetMissingItemsAsync(cancellationToken);

        if (missingItems.Count == 0)
            return new List<EgitimDatasetResponse>();

        var command = new CreateEgitimDatasetBulkCommand(
            missingItems.Select(x => new CreateEgitimDatasetRequest
            {
                TetikleyiciMetin = x.TetikleyiciMetin,
                TypeNum = x.TypeNum,
                sesTetikleyiciId = x.sesTetikleyiciId,
            }).ToList()
        );

        return await _mediator.Send(command, cancellationToken);
    }

    public async Task ExportEgitimDatasetToExcelAsync(CancellationToken cancellationToken)
    {
        var dataset = await _mediator.Send(new GetAllEgitimDatasetQuery(), cancellationToken);

        if (dataset is null || !dataset.Any())
            return;

        var filePath = @"C:\Users\berat\Desktop\voice_intent_dataset_numeric_package\voice_intent_dataset_numeric_tr.xlsx";

        using var workbook = File.Exists(filePath)
            ? new XLWorkbook(filePath)
            : new XLWorkbook();

        var worksheet = workbook.Worksheets.FirstOrDefault()
                        ?? workbook.Worksheets.Add("egitim_dataset");

        if (worksheet.FirstCellUsed() == null)
        {
            worksheet.Cell(1, 1).Value = "text";
            worksheet.Cell(1, 2).Value = "label";
        }

        var lastRow = worksheet.LastRowUsed()?.RowNumber() ?? 1;
        var row = lastRow + 1;

        foreach (var item in dataset)
        {
            worksheet.Cell(row, 1).Value = item.TetikleyiciMetin;
            worksheet.Cell(row, 2).Value = item.TypeNum;
            row++;
        }

        worksheet.Columns().AdjustToContents();
        workbook.Save();
    }

    public async Task<string> GetExcelPath(CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(@"C:\Users\berat\Desktop\voice_intent_dataset_numeric_package", "voice_intent_dataset_numeric_tr.xlsx");
        return filePath;
    }
}
