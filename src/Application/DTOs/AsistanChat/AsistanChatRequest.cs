using GeminiAsistanBackend.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.DTOs.AsistanChat;

public sealed class AsistanChatRequest
{
    public string Message { get; set; } = string.Empty;
    public AsistanYanitTuru asistanYanitTuru { get; set; }
    public int? SessionId { get; set; }
}


