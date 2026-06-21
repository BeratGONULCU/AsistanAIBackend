using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeminiAsistanBackend.Domain.Enums;

namespace GeminiAsistanBackend.Domain.Entities;

public class IslemLog
{
    public int Id { get; set; }
    public string DuyulanSes { get; set; } = null!;
    public IslemDurum Durum { get; set; } 
    public string? CevapMetni {  get; set; }
    public DateTime TarihSaat { get; set; } = DateTime.Now;
    public int? KomutId { get; set; }
    public string? raw_ai_json { get; set; }
    public CihazKomutu? Komut {  get; set; }
}

/*
 Table islem_loglari {
  id integer [primary key, increment]
  duyulan_ses varchar [not null]
  durum varchar [not null, note: 'YEREL_CALISTI, GEMINI_YALITTI, HATA']
  cevap_metni varchar
  tarih_saat datetime [default: `now()`, note: 'İşlem zamanı']
  komut_id integer [null, note: 'Alakasız isteklerde null kalır']
}

Table islem_loglari {
  id integer [primary key, increment]
  duyulan_ses varchar [not null]
  durum varchar [not null, note: 'YEREL_CALISTI, OLLAMA_YALITTI, OLLAMA_AI_LEARNED, HATA']
  cevap_metni varchar
  tarih_saat datetime [default: `now()`, note: 'İşlem zamanı']
  komut_id integer [null, note: 'Alakasız isteklerde null kalır']
  raw_ai_json text [null]
}
 
 */