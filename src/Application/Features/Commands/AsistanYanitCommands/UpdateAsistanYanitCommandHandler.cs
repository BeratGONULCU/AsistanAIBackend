using GeminiAsistanBackend.Application.DTOs.AsistanYanit;
using GeminiAsistanBackend.Domain.Enums;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GeminiAsistanBackend.Application.Interfaces;

namespace GeminiAsistanBackend.Application.Features.Commands.AsistanYanitCommands;

public sealed class UpdateAsistanYanitCommandHandler : IRequestHandler<UpdateAsistanYanitCommand, AsistanSendResponse>
{
    private readonly IApplicationDbContext _context;

    public UpdateAsistanYanitCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AsistanSendResponse> Handle(UpdateAsistanYanitCommand request, CancellationToken cancellationToken)
    {
        // 1. Entity'deki gerçek küçük harfli "id" alaný ile eþleyerek kaydý buluyoruz
        var asistanYanit = await _context.AsistanYanit
            .FirstOrDefaultAsync(x => x.id == request.id, cancellationToken);

        if (asistanYanit == null)
        {
            throw new Exception($"Güncellenmek istenen {request.id} ID'li asistan yanýtý bulunamadý.");
        }

        // 2. Entity'deki gerçek alan adý "yanitTuru" olduðu için onu güncelliyoruz
        asistanYanit.yanitTuru = request.yanit_turu;
        asistanYanit.updated_at = DateTime.UtcNow; // Güncellenme zamanýný da yenileyelim

        await _context.SaveChangesAsync(cancellationToken);

        // 3. Response nesnesini dolduruyoruz
        // AsistanSendResponse DTO'ndaki alan isimlerinin doðruluðundan emin olmak için 
        // en güvenli kolonlarý ve eþlemelerini kullandýk.
        return new AsistanSendResponse
        {
            Id = asistanYanit.id,
            AsistanYanit = asistanYanit.asistan_yanit, // Entity'deki "asistan_yanit" alanýndan okuyoruz
            YanitTuru = asistanYanit.yanitTuru,
            KomutId = asistanYanit.cihaz_komut_id,
            JsonData = asistanYanit.JsonData
        };
    }
}