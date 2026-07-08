using GeminiAsistanBackend.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.DTOs.SesTetikleyici;

public class SesTetikleyiciResponse
{
    public int Id { get; set; }
    public string TetikleyiciMetin { get; set; }
    // public int KomutId { get; set; }
    public EklenmeTuru? EklenmeTuru { get; set; }
    public double? llmConfidenceScore { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
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