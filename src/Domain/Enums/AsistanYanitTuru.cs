using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Domain.Enums;

public enum AsistanYanitTuru
{
    YANIT, // girilen komuta karşılık gelecek cevabın tipi
    KOMUT, // ilk girildiğinde bu
    CHAT,
    ACIKLAMA,
    DUZELTME, 
    REDMINE,
    PENDING, // cevap ui içerisinden geldiyse bu yazılır
    FEEDBACK, // gelen cevap doğru değilse bu gelir
    HATA
}
