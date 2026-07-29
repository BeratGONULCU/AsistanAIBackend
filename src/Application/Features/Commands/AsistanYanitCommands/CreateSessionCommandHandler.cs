using GeminiAsistanBackend.Application.DTOs.AsistanYanit;
using GeminiAsistanBackend.Application.Interfaces;
using GeminiAsistanBackend.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Commands.AsistanYanitCommands;

public sealed class CreateSessionCommandHandler : IRequestHandler<CreateSessionCommand, AsistanSendResponse>
{
    private readonly IApplicationDbContext _context;

    public CreateSessionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AsistanSendResponse> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {
        var asistanYanit = request.asistan_yanit?.Trim() ?? throw new ArgumentNullException(nameof(request.asistan_yanit));
        var yanitTuru = request.yanit_turu;

        // Son SessionId bulma mantığınız gayet başarılı ve performanslı
        int lastSessionID = (await _context.AsistanYanit.MaxAsync(x => (int?)x.SessionId, cancellationToken)) ?? 0;
        int newSessionID = lastSessionID + 1;

        var entities = new AsistanYanit
        {
            asistan_yanit = asistanYanit,
            yanitTuru = yanitTuru,
            SessionId = newSessionID,
        };

        await _context.AsistanYanit.AddAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new AsistanSendResponse
        {
            Id = entities.id,
            AsistanYanit = asistanYanit,
            YanitTuru = yanitTuru,
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