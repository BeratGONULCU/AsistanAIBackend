using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.DTOs.CihazKomut;

public class CihazKomutuResponse 
{
    public int Id { get; set; }
    public string type { get; set; } = null!;
    public string? domain { get; set; }
    public string? target { get; set; }
    public string operation { get; set; } = null!;

    public string? CalisacakKod { get; set; }
    public string? Aciklama {  get; set; } 

    // burada ICollection kısımları tanımlanmalı mı?
}


/* 
 
public class CihazKomutu
{
    public int Id { get; set; }
    public string AksiyonAnahtari { get; set; } = null!; // unique, not null
    public string CalisacakKod { get; set; } = null!;   // not null (Windows/system command)
    public string? Aciklama { get; set; }

    public ICollection<SesTetikleyicisi> SesTetikleyicileri { get; set; } = new List<SesTetikleyicisi>();
    public ICollection<IslemLog> IslemLoglari { get; set; } = new List<IslemLog>();
}

 */