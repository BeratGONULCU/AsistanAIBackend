using GeminiAsistanBackend.Application.DTOs.AsistanYanit;
using GeminiAsistanBackend.Application.Interfaces;
using GeminiAsistanBackend.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Commands.AsistanYanitCommands;

public sealed class CreateSessionCommandHandler : IRequestHandler<CreateSessionCommand, AsistanSendResponse>
{
    private readonly IApplicationDbContext _context;

    public CreateSessionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AsistanSendResponse> Handle(CreateSessionCommand request,CancellationToken cancellationToken)
    {
        var asistanYanit = request.asistan_yanit.Trim() ?? throw new ArgumentException(nameof(request.asistan_yanit));
        var YanitTuru = request.yanit_turu;

        /*
         * bu kısım çalışmıyordu
         
        int lastSessionID = await _context.AsistanYanit
            .Select(x => x.SessionId)
            .DefaultIfEmpty(0)
            .MaxAsync(cancellationToken);
        */

        int lastSessionID = (await _context.AsistanYanit.MaxAsync(x => (int?)x.SessionId, cancellationToken)) ?? 0;

        int newSessionID = lastSessionID + 1;

        var entities = new AsistanYanit
        {
            asistan_yanit = asistanYanit,
            yanitTuru = YanitTuru,
            SessionId = newSessionID,
        };

        await _context.AsistanYanit.AddAsync(entities);
        await _context.SaveChangesAsync();

        return new AsistanSendResponse
        {
            Id = entities.id,
            AsistanYanit = asistanYanit,
            YanitTuru = YanitTuru,
            SessionID = entities.SessionId,
            feedback = entities.feedback,
            CreatedAt = entities.created_at,
            UpdatedAt = entities.updated_at,
            KomutId = entities.cihaz_komut_id
        };
    }

}
