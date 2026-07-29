using GeminiAsistanBackend.Application.DTOs.AsistanYanit;
using GeminiAsistanBackend.Application.Interfaces;
using GeminiAsistanBackend.Domain.Entities;
using GeminiAsistanBackend.Domain.Enums;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Commands.AsistanYanitCommands;

public sealed class CreateAsistanYanitCommandHandler : IRequestHandler<CreateAsistanYanitCommand, AsistanSendResponse>
{
    private readonly IApplicationDbContext _context;

    public CreateAsistanYanitCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AsistanSendResponse> Handle(CreateAsistanYanitCommand request, CancellationToken cancellationToken)
    {
        var asistanYanit = request.asistan_yanit?.Trim() ?? throw new ArgumentNullException(nameof(request.asistan_yanit));
        AsistanYanitTuru yanitTuru = request.yanit_turu;
        int? cihazKomutID = request.cihaz_komut_id;

        var entities = new AsistanYanit
        {
            asistan_yanit = asistanYanit,
            cihaz_komut_id = cihazKomutID,
            yanitTuru = yanitTuru,
            SessionId = request.session_id,
            feedback = request.feedback,
            JsonData = request.JsonData
        };

        await _context.AsistanYanit.AddAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new AsistanSendResponse
        {
            Id = entities.id,
            AsistanYanit = asistanYanit,
            YanitTuru = yanitTuru,
            SessionId = entities.SessionId,
            RawResponse = request.raw_response ?? (entities.JsonData.HasValue ? entities.JsonData.Value.GetRawText() : null),
            Feedback = entities.feedback,
            KullaniciGeriBildirimi = entities.KullaniciGeriBildirimi,
            CreatedAt = entities.created_at,
            UpdatedAt = entities.updated_at,
            KomutId = entities.cihaz_komut_id,
            JsonData = entities.JsonData
        };
    }
}