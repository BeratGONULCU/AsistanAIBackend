using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.DTOs.TetikleyiciKomutlar;

public class CreateTetikleyiciKomutRequest
{
    public int tetikleticiId { get; set; }
    public int komutId { get; set; }
}
