namespace GeminiAsistanBackend.Application.DTOs.RedmineEgitimdataset;

public class CreateRedmineDatasetRequest
{
    public string redmine_tetikleyici_metin { get; set; } = null!;
    public string action { get; set; } = null!;
    public int sesTetikleyici_id { get; set; }
}