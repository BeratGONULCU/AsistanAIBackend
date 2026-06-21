using GeminiAsistanBackend.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.DTOs.IslemLog;

public class IslemLogResponse
{
    public int Id { get; set; }
    public string DuyulanSes { get; set; } = string.Empty;
    public IslemDurum Durum { get; set; }
    public string? CevapMetni { get; set; }
    public DateTime TarihSaat { get; set; }
    public int? KomutId { get; set; }
    public string? raw_ai_json { get; set; }

}
