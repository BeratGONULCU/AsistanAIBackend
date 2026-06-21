using GeminiAsistanBackend.Application.DTOs.EgitimDataset;
using System.IO;

namespace GeminiAsistanBackend.Application.Services;

public interface IEgitimDatasetSyncService
{
    Task<List<CreateEgitimDatasetRequest>> GetMissingItemsAsync(CancellationToken cancellationToken);
    Task<List<EgitimDatasetResponse>> SyncAsync(CancellationToken cancellationToken);
    Task ExportEgitimDatasetToExcelAsync(CancellationToken cancellationToken);
    Task<string> GetExcelPath(CancellationToken cancellationToken);
}