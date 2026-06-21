using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.DTOs;

public class ExcelImportResponse
{
    public string TetikleyiciMetin { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Domain { get; set; } = string.Empty;
    public string Target { get; set; } = string.Empty;
    public string Operation { get; set; } = string.Empty;
    public string CalisacakKod { get; set; } = string.Empty;
    public double Confidence { get; set; }
}