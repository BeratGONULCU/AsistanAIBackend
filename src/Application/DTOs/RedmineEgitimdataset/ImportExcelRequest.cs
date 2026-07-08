using Microsoft.AspNetCore.Http;

namespace GeminiAsistanBackend.Application.DTOs.RedmineEgitimdataset;

public class ImportExcelRequest
{
    public IFormFile File { get; set; } = default!;
}