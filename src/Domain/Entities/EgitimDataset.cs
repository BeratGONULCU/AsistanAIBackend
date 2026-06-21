using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Domain.Entities;

public class EgitimDataset
{
    public int Id { get; set; }
    public string tetikleyici_metin { get; set; } = null!;
    [Required(ErrorMessage = "type_num değeri boş kalamaz.")]
    public int type_num { get; set; }
    public int sesTetikleyici_id { get; set; }
    public SesTetikleyicisi SesTetikleyicisi { get; set; } = null!;
}

