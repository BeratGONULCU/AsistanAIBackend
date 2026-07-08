using GeminiAsistanBackend.Application.DTOs.CihazKomut;
using GeminiAsistanBackend.Application.Features.Commands.CihazKomutuCommands;
using GeminiAsistanBackend.Application.Interfaces;
using GeminiAsistanBackend.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GeminiAsistanBackend.Application.Commands;

public sealed class UpdateCihazKomutuCommandHandler
    : IRequestHandler<UpdateCihazKomutuCommand, CihazKomutuResponse?>
{
    private readonly IApplicationDbContext _context;

    public UpdateCihazKomutuCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CihazKomutuResponse?> Handle(
        UpdateCihazKomutuCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _context.CihazKomutlari
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity is null)
            return null;

        var type = request.type.Trim();
        var domain = request.domain?.Trim();
        var target = request.target?.Trim();
        var operation = request.operation.Trim();
        var calisacakKod = request.CalisacakKod?.Trim() ?? string.Empty;
        var aciklama = request.Aciklama?.Trim();

        if (string.IsNullOrWhiteSpace(type))
            throw new InvalidOperationException("komut tipi boş olamaz");

        if (string.IsNullOrWhiteSpace(operation))
            throw new InvalidOperationException("operation boş olamaz");

        entity.type = type;
        entity.domain = domain;
        entity.target = target;
        entity.operation = operation;
        entity.CalisacakKod = calisacakKod;
        entity.Aciklama = aciklama;

        await _context.SaveChangesAsync(cancellationToken);

        return new CihazKomutuResponse
        {
            Id = entity.Id,
            type = entity.type,
            domain = entity.domain,
            target = entity.target,
            operation = entity.operation,
            CalisacakKod = entity.CalisacakKod,
            Aciklama = entity.Aciklama
        };
    }
}