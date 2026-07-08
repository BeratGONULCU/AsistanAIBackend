using GeminiAsistanBackend.Application.DTOs.SesTetikleyici;
using GeminiAsistanBackend.Domain.Entities;
using GeminiAsistanBackend.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Interfaces.Repositories;

public interface ISesTetikleyiciRepository : IGenericRepository<SesTetikleyicisi>
{
    Task<SesTetikleyicisi> CompareTetikleyiciMetin(string compareMetin);
    // bu method içerisinde tetikleyicimetin değeri içerisinde python içerisindeki algoritma ile benzerlik araması yapacak.
    // ama burada tüm db değerlerini taramak uzun sürebilir.
    Task<IReadOnlyCollection<SesTetikleyiciResponse>> GetSesTetikleyiciByType(string type, CancellationToken cancellationToken);
    Task<int> CountTetikleyiciByType(string type, CancellationToken cancellationToken);

    // eklenmeturu ile arama için enum mı dönmesi alması gerek
    Task<IReadOnlyCollection<SesTetikleyiciResponse>> GetSesTetikleyiciByEklenmeTuru(EklenmeTuru eklenmeturu, CancellationToken cancellationToken);
}


/*
    public int Id { get; set; }
    public string TetikleyiciMetin { get; set; } = null!;
    public int KomutId { get; set; }
    public CihazKomutu Komut { get; set; } = null!;
    public EklenmeTuru? EklenmeTuru { get; set; } 


    bu tablo içerisinde tetikleyicimetin kısmı base control verimiz olacak.

    yani ;
    1 - ses verisi metine çevrilecek
    2 - db içerisinden ses_tetikleyicileri tablosundaki her bir TetikleyiciMetin verisi ile girilen metin verisi karşılaştırılacak.
    3 - benzerlik oranı en yüksek olan veri satırı seçilir. oran belirli bir score değerinden yüksek olmazsa işlem tekrarlanır.

*/