using GeminiAsistanBackend.Application.DTOs.AsistanYanit;
using GeminiAsistanBackend.Application.Interfaces;
using GeminiAsistanBackend.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Queries.AsistanYanitQueries;

public sealed class GetSohbetBySessionIDQueryHandler : IRequestHandler<GetSohbetBySessionIDQuery, AsistanSendResponse>
{
    private readonly IUnitOfWork _unitofwork;

    public GetSohbetBySessionIDQueryHandler(IUnitOfWork unitofwork)
    {
        _unitofwork = unitofwork;
    }
    
    public async Task<AsistanSendResponse> Handle(GetSohbetBySessionIDQuery request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var entities = await _unitofwork.AsistanYanit.GetByIdAsync(request.sessionId);

        if (entities == null)
        {
            return null;
        }

        var asistanYanit = entities.asistan_yanit.ToString();

        if (!Enum.TryParse<AsistanYanitTuru>(entities.yanitTuru.ToString(), true, out var yanitTuruEnum))
        {
            // Eğer çevirme başarısız olursa bir default değer atayabilirsiniz
            yanitTuruEnum = AsistanYanitTuru.YANIT;
        }

        return new AsistanSendResponse
        {
            Id = entities.id,
            AsistanYanit = entities.asistan_yanit,
            YanitTuru = yanitTuruEnum,
            SessionId = entities.SessionId,
            RawResponse = entities.JsonData.HasValue ? entities.JsonData.Value.GetRawText() : null,
            Feedback = entities.feedback,
            KullaniciGeriBildirimi = entities.KullaniciGeriBildirimi,
            CreatedAt = entities.created_at,
            UpdatedAt = entities.updated_at,
            KomutId = entities.cihaz_komut_id,
            JsonData = entities.JsonData
        };
    }
}


