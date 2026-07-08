using GeminiAsistanBackend.Application.DTOs.AsistanYanit;
using GeminiAsistanBackend.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Queries.AsistanYanitQueries;

public sealed class GetAsistanYanitQueryHandler : IRequestHandler<GetAsistanYanitQuery, List<AsistanSendResponse>>
{
    private readonly IUnitOfWork _unitofwork;

    public GetAsistanYanitQueryHandler(IUnitOfWork unitofwork)
    {
        _unitofwork = unitofwork;
    }

    public async Task<List<AsistanSendResponse>> Handle(GetAsistanYanitQuery request, CancellationToken cancellationToken)
    {
        var entities = await _unitofwork.AsistanYanit.GetAllAsync(cancellationToken);

        return entities
            .OrderByDescending(x => x.id)
            .Select(x => new AsistanSendResponse
            {
                Id = x.id,
                AsistanYanit = x.asistan_yanit,
                YanitTuru = x.yanitTuru,
                SessionID = x.SessionId,
                CreatedAt = x.created_at,
                UpdatedAt = x.updated_at,
                KomutId = x.cihaz_komut_id
            }).ToList();
    }

}
