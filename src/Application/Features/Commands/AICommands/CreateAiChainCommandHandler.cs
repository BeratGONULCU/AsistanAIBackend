using GeminiAsistanBackend.Application.Commands;
using GeminiAsistanBackend.Application.Features.Commands.CihazKomutuCommands;
using GeminiAsistanBackend.Application.Features.Commands.SesTetikleyiciKomutCommands;
using GeminiAsistanBackend.Application.Features.Commands.TetikleyiciKomutCommands;
using GeminiAsistanBackend.Application.Interfaces;
using GeminiAsistanBackend.Domain.Enums;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Commands.AICommands;

public sealed class CreateAiChainCommandHandler : IRequestHandler<CreateAiChainCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator; // Handler'ları tetiklemek için ekledik

    public CreateAiChainCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<bool> Handle(CreateAiChainCommand request, CancellationToken cancellationToken)
    {
        // 1. Ortak tek bir Transaction paketi açıyoruz
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            // 2. Senin yazdığın SesTetikleyiciHandler'ı tetikliyoruz (Validasyonlar ve Trim'ler çalışır)
            var sesResult = await _mediator.Send(new CreateSesTetikleyiciCommand
            (
                request.TetikleyiciMetin,
                EklenmeTuru.AI_LEARNED,
                request.Confidence
            ), cancellationToken);

            // 3. Senin yazdığın CihazKomutuHandler'ı tetikliyoruz 
            var komutResult = await _mediator.Send(new CreateCihazKomutuCommand
            (
                request.Type,
                request.Domain,
                request.Target,
                request.Operation,
                request.CalisacakKod,
                "{}"
            ), cancellationToken);

            // 4. İŞİN SIRRI: Yukarıdaki handler'lar çalışıp SaveChanges yaptığı için 
            // artık sesResult.Id ve komutResult.Id değerleri gerçek DB Id'leri olarak dolu geldi!

            // 5. İlişki tablosunu tetikliyoruz
            await _mediator.Send(new CreateTetikleyiciKomutCommand(
                sesResult.Id,
                komutResult.Id
            ), cancellationToken);

            // 6. Üç işlem de başarıyla bittiyse tek seferde bütünü onaylayıp diske yazıyoruz
            await _unitOfWork.CommitAsync(cancellationToken);
            return true;
        }
        catch (Exception)
        {
            // En ufak bir aşamada (örn: validasyon hatası) hata çıkarsa her şeyi geri al, DB'yi kirletme
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}