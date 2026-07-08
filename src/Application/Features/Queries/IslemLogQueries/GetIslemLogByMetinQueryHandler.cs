using GeminiAsistanBackend.Application.DTOs.IslemLog;
using GeminiAsistanBackend.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Queries.IslemLogQueries;

public class GetIslemLogByMetinQueryHandler : IRequestHandler<GetIslemLogByMetinQuery, List<IslemLogResponse?>>
{
    public readonly IUnitOfWork _unitOfWork;

    public GetIslemLogByMetinQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<List<IslemLogResponse?>> Handle(GetIslemLogByMetinQuery request, CancellationToken cancellationToken)
    {
        //var entities = await _unitOfWork.IslemLogRepository.GetByMetin(request.metin, cancellationToken);
        var entities = await _unitOfWork.IslemLoglari.GetByMetin(request.metin, cancellationToken);

        if (entities == null)
        {
            return null;
        }

        var response = entities.Select(entity => new IslemLogResponse {
            Id = entity.Id,
            DuyulanSes = entity.DuyulanSes,
            Durum = entity.Durum,
            CevapMetni = entity.CevapMetni,
            TarihSaat = entity.TarihSaat,
            KomutId = entity.KomutId,
            raw_ai_json = entity.raw_ai_json
        }).ToList();
        
        return response;
    }
}
