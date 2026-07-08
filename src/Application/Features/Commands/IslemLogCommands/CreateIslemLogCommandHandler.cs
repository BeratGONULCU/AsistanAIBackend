using GeminiAsistanBackend.Application.DTOs.IslemLog;
using GeminiAsistanBackend.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Commands.IslemLogCommands;

public sealed class CreateIslemLogCommandHandler : IRequestHandler<CreateIslemLogCommand, IslemLogResponse>
{
    public readonly IApplicationDbContext _context;

    public CreateIslemLogCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IslemLogResponse> Handle(CreateIslemLogCommand request, CancellationToken cancellationToken)
    {
        var DuyulanSes = request.DuyulanSes.Trim();
        var Durum = request.Durum;
        var raw_ai_json = request.raw_ai_json.Trim();   
        // entity içerisinde tarih gönderilecek.

        var entities = new Domain.Entities.IslemLog
        {
            DuyulanSes = DuyulanSes,
            Durum = Durum,
            CevapMetni = request.cevapMetni,
            TarihSaat = DateTime.UtcNow,
            KomutId = request.komutId,
            raw_ai_json = raw_ai_json,
        };

        await _context.IslemLoglari.AddAsync(entities,cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new IslemLogResponse
        {
            Id = entities.Id,
            DuyulanSes = DuyulanSes,
            Durum = entities.Durum,
            CevapMetni = entities.CevapMetni,
            TarihSaat = entities.TarihSaat,
            KomutId = entities.KomutId,
            raw_ai_json = entities.raw_ai_json
        };
    }


}
