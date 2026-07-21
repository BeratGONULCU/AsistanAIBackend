using System.ComponentModel.DataAnnotations;

namespace GeminiAsistanBackend.Application.DTOs.RedmineEgitimdataset;

public class CreateRedmineEgitimdatasetRequest
{
    [Required]
    public string RedmineTetikleyiciMetin { get; set; } = null!;

    [Required]
    public string Action { get; set; } = null!;

    [Required]
    public int SesTetikleyiciId { get; set; }

}