using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.DTOs.EgitimDataset;

public class EgitimDatasetResponse
{
    public int Id { get; set; }
    public string TetikleyiciMetin {  get; set; } = null!;
    public int TypeNum { get; set; }
    public int SesTetikleyiciId { get; set; }
}
