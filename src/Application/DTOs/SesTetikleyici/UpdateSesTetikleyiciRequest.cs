using GeminiAsistanBackend.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.DTOs.SesTetikleyici;

public class UpdateSesTetikleyiciRequest
{
    [Required(ErrorMessage = "Id zorunlu alan")]
    public int Id { get; set; }
    [Required(ErrorMessage = "TetikleyiciMetin zorunlu alan")]
    public string TetikleyiciMetin { get; set; } = null!;
    // public int KomutId { get; set; } 
    public EklenmeTuru? EklenmeTuru { get; set; }
}
