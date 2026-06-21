using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.DTOs.EgitimDataset;

public class CreateEgitimDatasetRequest
{
    [Required(ErrorMessage = "tetikleyici metin değeri zorunlu - request")]
    public string TetikleyiciMetin { get; set; } = null!;
    [Required(ErrorMessage = "typenum değeri girmek zorunludur")]
    public int? TypeNum { get; set; }
    [Required(ErrorMessage = "sesTetikleyiciId değeri girilmeli")]
    public int sesTetikleyiciId { get; set; } // int? + [Required] yaparak 0 gelmesini engelledik!
}
