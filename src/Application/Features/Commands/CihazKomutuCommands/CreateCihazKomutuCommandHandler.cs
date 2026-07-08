using GeminiAsistanBackend.Application.Interfaces;
using GeminiAsistanBackend.Application.DTOs.CihazKomut;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GeminiAsistanBackend.Application.Features.Commands.CihazKomutuCommands;

public sealed class CreateCihazKomutuCommandHandler
    : IRequestHandler<CreateCihazKomutuCommand,CihazKomutuResponse>
{
    private readonly IApplicationDbContext _context;

    public CreateCihazKomutuCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CihazKomutuResponse> Handle(CreateCihazKomutuCommand request,CancellationToken cancellationToken)
    {
        var type = request.type.Trim();
        var domain = request.domain?.Trim();
        var target = request.target?.Trim();
        var operation = request.operation.Trim();
        var calisacakKod = request.CalisacakKod?.Trim() ?? string.Empty;
        var aciklama = request.Aciklama?.Trim();

        if(string.IsNullOrEmpty(type))
        {
            throw new InvalidOperationException("komut tipi boş olamaz");
        }

        /*
        var ayniKayitVarMi = await _unitOfWork.CihazKomutlari.AnyAsync(
            x => x.type == type,
            cancellationToken);
        

        if (ayniKayitVarMi)
            throw new InvalidOperationException("bu cihaz komutu zaten kayıtlı");
        */

        var calisacakKodVarMi = await _context.CihazKomutlari.AnyAsync(
            x => x.CalisacakKod == calisacakKod,
            cancellationToken);

        if(calisacakKod != null)
        {
            //if (calisacakKodVarMi)
                //throw new InvalidOperationException("girilen calisacak kod değeri kayıtlı");
                
        }

        var entity = new Domain.Entities.CihazKomutu
        {
            type = type,
            domain = domain,
            target = target,
            operation = operation,
            CalisacakKod = calisacakKod,
            Aciklama = aciklama
        };

        await _context.CihazKomutlari.AddAsync(entity,cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new CihazKomutuResponse
        {
            Id = entity.Id,
            type = entity.type,
            domain = entity.domain,
            target = entity.target,
            operation = entity.operation,
            CalisacakKod = entity.CalisacakKod,
            Aciklama = entity.Aciklama,
        };
    }
}
