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

public sealed class GetAsistanYanitByIdQueryHandler : IRequestHandler<GetAsistanYanitByIdQuery , AsistanSendResponse?>
{
    private readonly IUnitOfWork _unitofwork;

    public GetAsistanYanitByIdQueryHandler(IUnitOfWork unitofwork)
    {
        _unitofwork = unitofwork;
    }

    public async Task<AsistanSendResponse?> Handle(GetAsistanYanitByIdQuery request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        // 1. Veriyi çekiyoruz
        var entities = await _unitofwork.AsistanYanit.GetByIdAsync(request.id);

        // 2. ÖNCE null kontrolü yapıyoruz (Çökmeyi önlemek için)
        if (entities == null)
        {
            return null;
        }

        // 3. Null olmadığından emin olduktan sonra değerleri okuyoruz
        var asistanYanit = entities.asistan_yanit?.ToString() ?? string.Empty;

        // 4. Enum dönüşüm kontrolü (Burası temiz)
        if (!Enum.TryParse<AsistanYanitTuru>(entities.yanitTuru.ToString(), true, out var yanitTuruEnum))
        {
            yanitTuruEnum = AsistanYanitTuru.YANIT;
        }

        return new AsistanSendResponse
        {
            Id = entities.id,
            AsistanYanit = entities.asistan_yanit,
            YanitTuru = yanitTuruEnum,
            SessionID = entities.SessionId, 
            feedback = entities.feedback,
            KomutId = entities.cihaz_komut_id
        };
    }

}
