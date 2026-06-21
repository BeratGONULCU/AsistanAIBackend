using System.ComponentModel.DataAnnotations;

namespace GeminiAsistanBackend.Application.DTOs.CihazKomut;

public class CreateCihazKomutuRequest
{
    [Required(ErrorMessage = "Komut tipi (type) alanı zorunludur.")]
    public string Type { get; set; } = null!;

    //[Required(ErrorMessage = "Domain alanı zorunludur.")]
    public string? Domain { get; set; } = null!;

    public string? Target { get; set; } 

    [Required(ErrorMessage = "Operation alanı zorunludur.")]
    public string Operation { get; set; } = null!;

    public string? CalisacakKod { get; set; }

    public string? Aciklama { get; set; }
}
