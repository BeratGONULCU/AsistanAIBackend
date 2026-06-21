using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Domain.Entities;

public class CihazKomutu
{
    public int Id { get; set; }
    //public string AksiyonAnahtari { get; set; } = null!; --> silindi
    public string type { get; set; } = null!; // note: question,command
    public string? domain { get; set; } // note: system, browser, camera
    public string? target { get; set; }  // note: ram, cpu, chrome, front_camera
    public string operation { get; set; } = null!; // note: get_info, set_info, open, close
    public string? CalisacakKod { get; set; } // note: Windows/Sistem terminal komutu
    public string? Aciklama {  get; set; }

    // public ICollection<SesTetikleyicisi> SesTetikleyicileri { get; set; } = new List<SesTetikleyicisi>();
    public ICollection<IslemLog> IslemLoglari { get; set; } = new List<IslemLog>();
    public ICollection<TetikleyiciKomut> TetikleyiciKomutlari { get; set; } = new List<TetikleyiciKomut>();
}


/*

Table cihaz_komutlari {
  id integer [primary key, increment]
  type varchar [not null, note: 'question,command']
  domain varchar [not null, note: 'Örn: system, browser, camera']
  target varchar [not null, note: 'Örn: ram, cpu, chrome, front_camera']
  operation varchar [not null, note: 'Örn: get_info, set_info, open, close']
  calisacak_kod varchar [not null, note: 'Windows/Sistem terminal komutu']
  aciklama varchar [note: 'Komutun işlevi']
  
  // Aynı niyetin mükerrer eklenmesini önlemek için composite unique index
  Note: 'domain, target ve operation üçlüsü benzersiz olmalı'
}
 
 */