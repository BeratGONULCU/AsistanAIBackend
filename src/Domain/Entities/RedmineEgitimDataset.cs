using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Domain.Entities;

public class RedmineEgitimDataset
{
    public int Id { get; set; }
    public string? redmine_tetikleyici_metin { get; set; } = null!;
    public string? action { get; set; } = null!;
    public int sesTetikleyici_id { get; set; }
    public SesTetikleyicisi sesTetikleyicisi { get; set; } = null!;
}


/*
 
 Table redmine_egitim_dataset{
  id integer [primary key, increment]
  tetikleyici_metin varchar [unique] // cümle
  action varchar [not null] // işlem
  sesTetikleyici_id integer [not null]
}

 */