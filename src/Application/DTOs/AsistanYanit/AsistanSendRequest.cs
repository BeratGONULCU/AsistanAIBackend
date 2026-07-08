using GeminiAsistanBackend.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.DTOs.AsistanYanit;

public class AsistanSendRequest
{
    public string AsistanYanit { get; set; } = null!;
    //public AsistanYanitTuru YanitTuru { get; set; }
    public int? KomutId { get; set; }
    public int SessionId { get; set; }
    public string? feedback { get; set; } = null;
}
