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
    FEEDBACK, // bu redmine_egitim_dataset içine atılmış durumda demek
    HATA,
    FEEDBACKHATA, // bu redmine eğitilecek veriler için ,
    ONAY,
    ONAYYANIT
}
