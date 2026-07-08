using GeminiAsistanBackend.Application.Features.Commands;
using GeminiAsistanBackend.Application.Features.Commands.AICommands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AiChainController : ControllerBase
{
    private readonly IMediator _mediator;

    public AiChainController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Yapay zekadan gelen metin, komut ve kod bloklarını tek bir transaction altında zincirleme kaydeder.
    /// </summary>
    /// <param name="command">Yapay zeka zincir verileri</param>
    [HttpPost("create-chain")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAiChain(
        [FromBody] CreateAiChainCommand command,
        CancellationToken cancellationToken)
    {
        if (command == null)
        {
            return BadRequest(new { Success = false, Message = "İstek verisi boş olamaz." });
        }

        // Gönderilen record doğrudan MediatR üzerinden Handler'a iletilir.
        var result = await _mediator.Send(command, cancellationToken);

        if (result)
        {
            return Ok(new { Success = true, Message = "Yapay zeka komut zinciri başarıyla oluşturuldu." });
        }

        return BadRequest(new { Success = false, Message = "Komut zinciri kaydedilirken bir hata oluştu." });
    }

    // Bu method seçilen excel dosyasını veritabanına kaydetmek için yazıldı.
    // ExcelWriteDataCommand içerisine dosyanın stream nesnesini, dosya adını, dosya boyutunu gönderiyoruz.
    [HttpPost("import-excel")] 
    public async Task<IActionResult> ImportExcel(IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0) return BadRequest("Dosya seçilmedi.");

        // Dosyayı API katmanında stream'e çevirip Application katmanına öyle fırlatıyoruz
        using var stream = file.OpenReadStream();

        // burada örnek excel dosya yolu : "C:\Users\berat\Desktop\datasets\voice_intent_dataset_numeric_package\test.xlsx"
        var result = await _mediator.Send(new ExcelWriteDataCommand(stream,file.FileName,file.Length), cancellationToken);

        if (result) return Ok("Başarılı");
        return BadRequest("Başarısız");
    }
}