using GeminiAsistanBackend.Application.DTOs.SesTetikleyici;
using GeminiAsistanBackend.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Queries.SesTetikleyicileriQueries;

public class CheckDuplicateSesTetikleyiciQueryHandler : IRequestHandler<CheckDuplicateSesTetikleyiciQuery, bool?>
{
    public readonly IUnitOfWork _unitOfWork;

    public CheckDuplicateSesTetikleyiciQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool?> Handle(
    CheckDuplicateSesTetikleyiciQuery request,
    CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.metin))
            return false;

        // Arama metnini temizle ve küçült
        var aranacakMetin = request.metin.Trim().ToLower();

        // Veritabanındaki boşlukları esnetmek için Contains (İçeriyor mu) 
        // ya da birebir eşleşme için her iki tarafı da ToLower() yaparak karşılaştırıyoruz.
        var entities = await _unitOfWork.SesTetikleyicileri.AnyAsync(x =>
            x.TetikleyiciMetin.ToLower().Contains(aranacakMetin),
            cancellationToken);

        return entities;
    }
}
