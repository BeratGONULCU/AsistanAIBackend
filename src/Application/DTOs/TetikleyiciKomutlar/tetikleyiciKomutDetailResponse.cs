using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.DTOs.TetikleyiciKomutlar;

public class tetikleyiciKomutDetailResponse
{
    public int TetikleticiId { get; set; }
    public int KomutId { get; set; }
    public string TetikleyiciMetin {  get; set; }
    public string AksiyonAnahtari {  get; set; }
    public string CalisacakKod {  get; set; }
}
