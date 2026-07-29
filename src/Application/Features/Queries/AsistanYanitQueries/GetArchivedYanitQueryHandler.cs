using GeminiAsistanBackend.Application.DTOs.AsistanYanit;
using GeminiAsistanBackend.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Queries.AsistanYanitQueries;

public sealed class GetArchivedYanitQueryHandler : IRequestHandler<GetArchivedYanitQuery, List<AsistanSendResponse>>
{
    private readonly IUnitOfWork _unitofwork;

    public GetArchivedYanitQueryHandler(IUnitOfWork unitofwork)
    {
        _unitofwork = unitofwork;
    }

    public async Task<List<AsistanSendResponse>> Handle(GetArchivedYanitQuery request, CancellationToken cancellationToken)
    {
        var entities = await _unitofwork.AsistanYanit.GetAllAsync(cancellationToken);

        return entities
            .Where(x => x.IsArchived == true)
            .OrderByDescending(x => x.id)
            .Select(x => new AsistanSendResponse
            {
                Id = x.id,
                AsistanYanit = x.asistan_yanit,
                YanitTuru = x.yanitTuru,
                SessionId = x.SessionId,
                //RawResponse = x.JsonData.HasValue ? x.JsonData.Value.GetRawText() : null, 
                Feedback = x.feedback,
                KullaniciGeriBildirimi = x.KullaniciGeriBildirimi,
                CreatedAt = x.created_at,
                UpdatedAt = x.updated_at,
                KomutId = x.cihaz_komut_id,
                //JsonData = x.JsonData
            })
            .ToList();

    }
        
}
