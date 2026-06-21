using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Domain.Entities;
public class TetikleyiciKomut
{
    public int TetikleyiciId { get; set; }
    public SesTetikleyicisi Tetikleyici { get; set; } = null!;

    public int KomutId { get; set; }
    public CihazKomutu Komut { get; set; } = null!;

    // public DbSet<TetikleyiciKomut> TetikleyiciKomut {  get; set; }
}
