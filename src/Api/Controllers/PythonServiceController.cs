using GeminiAsistanBackend.Application.DTOs.AsistanChat;
using GeminiAsistanBackend.Application.Interfaces.Python;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class PythonController : ControllerBase
{
    private readonly IPythonService _pythonService;
    private readonly IPythonRunService _pythonRunService;

    public PythonController(IPythonService pythonService , IPythonRunService pythonRunService)
    {
        _pythonService = pythonService;
        _pythonRunService = pythonRunService;
    }

    [HttpPost("run-python")]
    public async Task<ActionResult> pythonExecute()
    {
        try
        {
            var result = await _pythonRunService.RunPythonMainScriptAsync();
            return Ok(new { output = result });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("send")]
    public async Task<ActionResult<AsistanChatResponse>> Send(
        [FromBody] AsistanChatRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
        {
            return BadRequest(new
            {
                ok = false,
                message = "Mesaj boş olamaz."
            });
        }
        
        /*
         * Not:
         * Session oluşturma ve kullanıcı KOMUT kaydı şu an sende zaten backend tarafında var.
         *
         * İdeal yapı:
         * - sessionId null/0 ise create-sessionID command mantığını burada çağır.
         * - sessionId varsa send-asistan-komut command mantığını burada çağır.
         *
         * Aşağıdaki kısım şu an Python'a payload gönderme tarafını gösteriyor.
         */

        var sessionId = request.SessionId.GetValueOrDefault();

        if (sessionId <= 0)
        {
            /*
             * Burada kendi create-sessionID command'ini çağırmalısın.
             * Örnek:
             *
             * var createdSession = await _mediator.Send(
             *     new CreateSessionCommand(request.Message, "KOMUT"),
             *     cancellationToken
             * );
             *
             * sessionId = createdSession.SessionID;
             */

            sessionId = 0; // Bunu kendi CreateSessionCommand sonucuyla dolduracaksın.
        }
        else
        {
            /*
             * Burada kendi send-asistan-komut command'ini çağırmalısın.
             * Örnek:
             *
             * await _mediator.Send(
             *     new SendAsistanKomutCommand(request.Message, "KOMUT", sessionId),
             *     cancellationToken
             * );
             */
        }

        var requestId = Guid.NewGuid().ToString("N");

        var pythonPayload = new
        {
            requestId,
            asistanYanit = request.Message.Trim(),
            yanitTuru = "KOMUT",
            sessionId
        };

        string pythonRawResult;

        try
        {
            pythonRawResult = await _pythonService.RunMainScriptAsync(pythonPayload);
        }
        catch (Exception ex)
        {
            return BadRequest(new AsistanChatResponse
            {
                Ok = false,
                SessionId = sessionId,
                UserText = request.Message,
                AssistantResponse = "",
                Message = $"Python çalıştırılırken hata oluştu: {ex.Message}"
            });
        }

        var assistantResponse = ExtractAssistantResponse(pythonRawResult);

        return Ok(new AsistanChatResponse
        {
            Ok = true,
            SessionId = sessionId,
            UserText = request.Message,
            AssistantResponse = assistantResponse,
            Message = "Komut işlendi."
        });
    }

    private static string ExtractAssistantResponse(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return "";

        try
        {
            using var doc = JsonDocument.Parse(raw);
            var root = doc.RootElement;

            if (root.TryGetProperty("assistantResponse", out var assistantResponse))
                return assistantResponse.GetString() ?? "";

            if (root.TryGetProperty("asistanCevabi", out var asistanCevabi))
                return asistanCevabi.GetString() ?? "";

            if (root.TryGetProperty("cevap", out var cevap))
                return cevap.GetString() ?? "";

            if (root.TryGetProperty("answer", out var answer))
                return answer.GetString() ?? "";

            if (root.TryGetProperty("output", out var output))
            {
                var outputText = output.GetString();

                if (string.IsNullOrWhiteSpace(outputText))
                    return "";

                try
                {
                    using var innerDoc = JsonDocument.Parse(outputText);
                    var innerRoot = innerDoc.RootElement;

                    if (innerRoot.TryGetProperty("assistantResponse", out var innerAssistantResponse))
                        return innerAssistantResponse.GetString() ?? "";

                    if (innerRoot.TryGetProperty("cevap", out var innerCevap))
                        return innerCevap.GetString() ?? "";

                    if (innerRoot.TryGetProperty("answer", out var innerAnswer))
                        return innerAnswer.GetString() ?? "";
                }
                catch
                {
                    return outputText;
                }

                return outputText;
            }

            return raw;
        }
        catch
        {
            return raw;
        }
    }
}


