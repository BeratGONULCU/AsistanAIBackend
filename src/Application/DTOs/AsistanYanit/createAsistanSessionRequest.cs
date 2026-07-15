using GeminiAsistanBackend.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.DTOs.AsistanYanit;

public class createAsistanSessionRequest
{
    public string AsistanYanit { get; set; } = null!;
    // public AsistanYanitTuru? YanitTuru { get; set; } = null;
    public AsistanYanitTuru YanitTuru { get; set; } = AsistanYanitTuru.KOMUT;
}



