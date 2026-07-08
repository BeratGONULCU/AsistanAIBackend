using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeminiAsistanBackend.Domain.Enums;

namespace GeminiAsistanBackend.Domain.Entities;

public class SesTetikleyicisi
{
    public int Id { get; set; }
    public string TetikleyiciMetin { get; set; } = null!;

    // public int KomutId { get; set; }
    //public CihazKomutu Komut { get; set; } = null!;
    public EklenmeTuru? EklenmeTuru { get; set; }  // MANUEL , AI_LEARNED
    // public double? ai_confidence_score { get; set; } // gelen veri 0 ile 1 arasında olacak
    public double? llm_confidence_score { get; set; } // gelen veri 0 ile 1 arasında olacak
    public DateTime created_at { get; set; } = DateTime.UtcNow;
    public DateTime updated_at {  get; set; } = DateTime.UtcNow;
    public ICollection<TetikleyiciKomut> TetikleyiciKomutlari { get; set; } = new List<TetikleyiciKomut>();
    public ICollection<EgitimDataset> EgitimDatasetleri { get; set; } = new List<EgitimDataset>();
    public ICollection<RedmineEgitimDataset> RedmineEgitimDatasets { get; set; } = new List<RedmineEgitimDataset>();
}


/*
 * 
 
Table ses_tetikleyicileri {
  id integer [primary key, increment]
  tetikleyici_metin varchar [unique, not null, note: 'Kullanıcının söylediği söz']
  //komut_id integer [not null]
  eklenme_turu varchar [note: 'MANUEL veya AI_LEARNED']
  ai_confidence_score integer [null, note: 'eklenme_turu = MANUEL ise null olur'] 
}
 
bu tablo içerisinde tetikleyicimetin kısmı base control verimiz olacak.

yani ;
1 - ses verisi metine çevrilecek
2 - db içerisinden ses_tetikleyicileri tablosundaki her bir TetikleyiciMetin verisi ile girilen metin verisi karşılaştırılacak.
3 - benzerlik oranı en yüksek olan veri satırı seçilir. oran belirli bir score değerinden yüksek olmazsa işlem tekrarlanır.

 */