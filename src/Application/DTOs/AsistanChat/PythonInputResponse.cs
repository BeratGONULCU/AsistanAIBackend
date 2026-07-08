using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.DTOs.AsistanChat;

public sealed class PythonInputResponse
{
    public bool? Ok { get; set; }
    public string? RequestId { get; set; }
    public int? SessionId { get; set; }
    public string? Status { get; set; }
    public string? Message { get; set; }
    public string? UserText { get; set; }
    public string? AssistantResponse { get; set; }
    public string? AsistanYanit { get; set; }
    public string? Output { get; set; }
    public object? RawAi { get; set; }
    public int? CommandId { get; set; }
}
