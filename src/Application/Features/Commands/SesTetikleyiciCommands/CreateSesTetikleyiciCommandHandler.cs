using GeminiAsistanBackend.Application.DTOs.SesTetikleyici;
using GeminiAsistanBackend.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeminiAsistanBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GeminiAsistanBackend.Application.Features.Commands.SesTetikleyiciKomutCommands;

public sealed class CreateSesTetikleyiciCommandHandler
    : IRequestHandler<CreateSesTetikleyiciCommand,SesTetikleyiciResponse>
{
    private readonly IApplicationDbContext _context;

    public CreateSesTetikleyiciCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SesTetikleyiciResponse> Handle(CreateSesTetikleyiciCommand request,CancellationToken cancellationToken)
    {
        var tetikleyiciMetin = request.TetikleyiciMetin.Trim();
        var eklenmeturu = request.EklenmeTuru;
        var aiConfidenceScore = request.aiConfidenceScore;

        if(string.IsNullOrEmpty(tetikleyiciMetin))
        {
            throw new InvalidOperationException("tetikleyici metin değeri boş olamaz.");
        }

        if(!eklenmeturu.HasValue)
        {
            throw new InvalidOperationException("eklenme türü değeri boş olamaz.");
        }

        if(aiConfidenceScore < 0.0 || aiConfidenceScore > 1.0)
        {
            throw new InvalidOperationException("aiConfidenceScore değeri 0 ile 1 arasında olması gerekiyor. ");
        }

        var ayniKayitVarMi = await _context.SesTetikleyicileri.AnyAsync(
            x => x.TetikleyiciMetin == tetikleyiciMetin,
            cancellationToken);


        //if (ayniKayitVarMi)
            //throw new InvalidOperationException("bu tetikleyici metin değeri kayıtlı");

        var entity = new SesTetikleyicisi
        {
            TetikleyiciMetin = tetikleyiciMetin,
            EklenmeTuru = eklenmeturu,
            llm_confidence_score = aiConfidenceScore
        };

        await _context.SesTetikleyicileri.AddAsync(entity,cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new SesTetikleyiciResponse
        { 
            Id = entity.Id,
            TetikleyiciMetin = entity.TetikleyiciMetin,
            EklenmeTuru = entity.EklenmeTuru,
            llmConfidenceScore = entity.llm_confidence_score
        };
    }
}
