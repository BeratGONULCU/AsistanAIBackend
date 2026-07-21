using GeminiAsistanBackend.Application.DTOs.AsistanYanit;
using GeminiAsistanBackend.Application.Interfaces;
using GeminiAsistanBackend.Domain.Entities;
using GeminiAsistanBackend.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Commands.AsistanYanitCommands;

public sealed class CreateAsistanYanitCommandHandler : IRequestHandler<CreateAsistanYanitCommand , AsistanSendResponse>
{
    private readonly IApplicationDbContext _context;

    public CreateAsistanYanitCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    // bu sadece sohbeti başlatmak için kullanılacak.
    public async Task<AsistanSendResponse> Handle(CreateAsistanYanitCommand request, CancellationToken cancellationToken)
    {
        var asistanYanit = request.asistan_yanit.Trim() ?? throw new ArgumentException(nameof(request.asistan_yanit));
        AsistanYanitTuru yanitTuru = request.yanit_turu;
        int? cihazKomutID = request.cihaz_komut_id;

        // Eksik olan atamaları tam olarak burada yapıyoruz:
        var entities = new AsistanYanit
        {
            asistan_yanit = asistanYanit,
            cihaz_komut_id = cihazKomutID,
            yanitTuru = yanitTuru,          // Enum türünü bağladık
            SessionId = request.session_id, // Command'den gelen session_id'yi bağladık
            feedback = request.feedback,     // Feedback alanını bağladık
            JsonData = request.JsonData,
        };

        await _context.AsistanYanit.AddAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new AsistanSendResponse
        {
            Id = entities.id,
            AsistanYanit = asistanYanit,
            YanitTuru = yanitTuru,
            RawResponse = request.raw_response,
            SessionID = entities.SessionId, // veritabanına yazılan gerçek SessionId dönecek
            feedback = entities.feedback,
            CreatedAt = entities.created_at,
            UpdatedAt = entities.updated_at,
            KomutId = entities.cihaz_komut_id,
            JsonData = entities.JsonData,
        };
    }

}
