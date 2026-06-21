using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeminiAsistanBackend.Application.DTOs.CihazKomut;
using GeminiAsistanBackend.Application.Interfaces;
using MediatR;

namespace GeminiAsistanBackend.Application.Queries;

public sealed class GetAllCihazKomutlariQueryHandler : IRequestHandler<GetAllCihazKomutlariQuery,List<CihazKomutuResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllCihazKomutlariQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;  
    }

    public async Task<List<CihazKomutuResponse>> Handle(
        GetAllCihazKomutlariQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.CihazKomutlari.GetAllAsync(cancellationToken);

        return entities
            .OrderBy(x => x.Id)
            .Select(x => new CihazKomutuResponse
            {
                Id = x.Id,
                type = x.type,
                domain = x.domain,
                target = x.target,
                operation = x.operation,
                CalisacakKod = x.CalisacakKod,
                Aciklama = x.Aciklama
            })
            .ToList();
    }
}
