using GeminiAsistanBackend.Application.DTOs.IslemLog;
using GeminiAsistanBackend.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Queries.IslemLogQueries;

public class GetIslemLogByDurumQueryHandler : IRequestHandler<GetIslemLogByDurumQuery, IEnumerable<IslemLogResponse?>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetIslemLogByDurumQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<IslemLogResponse?>> Handle(GetIslemLogByDurumQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.IslemLoglari.GetIslemLogByDurum(request.durum, cancellationToken);

        if (result == null)
        {
            return Enumerable.Empty<IslemLogResponse?>();
        }

        // şimdi bu response içerisinde request.durum olarak mı , x.durum olarak mı getirilecek?
        var response = result.Select(x => new IslemLogResponse
        {
            Id = x.Id,
            DuyulanSes = x.DuyulanSes,
            Durum = x.Durum,
            CevapMetni = x.CevapMetni,
            TarihSaat = x.TarihSaat,
            KomutId = x.KomutId,
            raw_ai_json = x.raw_ai_json,
        }).ToList();

        return response;    
    }
        
    

}
