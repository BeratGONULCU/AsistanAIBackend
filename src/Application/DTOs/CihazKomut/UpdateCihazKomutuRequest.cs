using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.DTOs.CihazKomut;

public class UpdateCihazKomutuRequest
{
    [Required(ErrorMessage = "id alanı zorunludur.")]
    public int Id {  get; set; }

    [Required(ErrorMessage = "type alanı zorunludur.")]
    public string Type { get; set; } = null!;

    public string? Domain { get; set; } 

    public string? Target { get; set; }

    public string? Operation { get; set; } 

    public string? CalisacakKod { get; set; }

    public string? Aciklama { get; set; }
}
