using GeminiAsistanBackend.Application.DTOs.TetikleyiciKomutlar;
using GeminiAsistanBackend.Application.Interfaces;
using GeminiAsistanBackend.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Commands.TetikleyiciKomutCommands;

public sealed class CreateTetikleyiciKomutCommandHandler : IRequestHandler<CreateTetikleyiciKomutCommand,TetikleyiciKomutReponse>
{
    public readonly IApplicationDbContext _context;

    public CreateTetikleyiciKomutCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TetikleyiciKomutReponse> Handle(CreateTetikleyiciKomutCommand request, CancellationToken cancellationToken)
    {
        var sesTetikleyiciId = request.tetikleyiciId;
        var komutId = request.komutId;

        if (string.IsNullOrEmpty(sesTetikleyiciId.ToString()))
        {
            throw new InvalidOperationException("sesTetikleyiciId değeri boş olamaz.");
        }

        if (string.IsNullOrEmpty(komutId.ToString()))
        {
            throw new InvalidOperationException("komutId değeri boş olamaz.");
        }

        var entities = new TetikleyiciKomut
        {
            TetikleyiciId = sesTetikleyiciId,
            KomutId = komutId,
        };

        await _context.TetikleyiciKomutlar.AddAsync(entities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new TetikleyiciKomutReponse
        {
            TetikleticiId = entities.TetikleyiciId,
            KomutId = entities.KomutId,
        };
    }
}
