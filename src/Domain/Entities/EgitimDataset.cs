using GeminiAsistanBackend.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Domain.Entities;

public class EgitimDataset
{
    /*
        ["question"] = 0,
        ["command"] = 1,
        ["chat"] = 2,
        ["info"] = 3,
        ["uncertain"] = 4
     */

    public int Id { get; set; }
    public string tetikleyici_metin { get; set; } = null!;
    [Required(ErrorMessage = "type_num değeri boş kalamaz.")]
    public int type_num { get; set; }
    public int sesTetikleyici_id { get; set; }
    public SesTetikleyicisi SesTetikleyicisi { get; set; } = null!;
}

