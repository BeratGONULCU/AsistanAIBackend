using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.DTOs.AsistanChat;

public sealed class AsistanChatResponse
{
    public bool Ok { get; set; }
    public int SessionId { get; set; }
    public string UserText { get; set; } = string.Empty;
    public string AssistantResponse { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}