using GeminiAsistanBackend.Application.DTOs.SesTetikleyici;
using GeminiAsistanBackend.Application.Features.Commands.SesTetikleyiciKomutCommands;
using GeminiAsistanBackend.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Commands.SesTetikleyiciCommands;

public sealed class UpdateSesTetikleyiciKomutCommandHandler : IRequestHandler<UpdateSesTetikleyiciKomutCommand,SesTetikleyiciResponse?>
{
    public readonly IApplicationDbContext _context;

    public UpdateSesTetikleyiciKomutCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SesTetikleyiciResponse?> Handle(UpdateSesTetikleyiciKomutCommand request,CancellationToken cancellationToken)
    {
        var result = await _context.SesTetikleyicileri.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (result == null)
        {
            return null;
        }

        if (request.TetikleyiciMetin.Trim() == null)
        {
            throw new InvalidOperationException("TetikleyiciMetin değeri boş");
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new SesTetikleyiciResponse
        {
            Id = result.Id,
            TetikleyiciMetin = result.TetikleyiciMetin,
            EklenmeTuru = result.EklenmeTuru,
            llmConfidenceScore = result.llm_confidence_score
        };
    }
}
