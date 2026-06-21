using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeminiAsistanBackend.Domain.Entities;
using GeminiAsistanBackend.Domain.Enums;

namespace GeminiAsistanBackend.Application.DTOs.SesTetikleyici;

public class CreateSesTetikleyiciRequest
{
    [Required(ErrorMessage = "TetikleyiciMetin zorunlu alan")]
    public string TetikleyiciMetin { get; set; } = null!;
    // public int KomutId { get; set; } 
    public EklenmeTuru? EklenmeTuru { get; set;}
    public double? aiConfidenceScore { get; set; }
}


/*
 public class SesTetikleyicisi
{
    public int Id { get; set; }
    public string TetikleyiciMetin { get; set; } = null!; // unique, not null

    // FK to CihazKomutu
    public int KomutId { get; set; }
    public CihazKomutu Komut { get; set; } = null!;

    public EklenmeTuru? EklenmeTuru { get; set; }
}
 
 */