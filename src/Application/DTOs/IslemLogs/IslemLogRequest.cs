using GeminiAsistanBackend.Domain.Enums;
using GeminiAsistanBackend.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.DTOs.IslemLog;

public class IslemLogRequest
{
    public string DuyulanSes { get; set; } = null!;
    public IslemDurum Durum { get; set; }
    public string? CevapMetni { get; set; }
    public DateTime TarihSaat {  get; set; }
    public int? KomutId { get; set; }
}

/*
public class IslemLog
{
public int Id { get; set; }
public string DuyulanSes { get; set; } = null!;
public IslemDurum Durum { get; set; }
public string? CevapMetni { get; set; }
public DateTime TarihSaat { get; set; }
public int? KomutId { get; set; }
public CihazKomutu? Komut { get; set; }
}
*/