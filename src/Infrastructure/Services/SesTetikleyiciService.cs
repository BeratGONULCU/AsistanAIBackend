using DocumentFormat.OpenXml.Drawing.Charts;
using GeminiAsistanBackend.Application.DTOs.SesTetikleyici;
using GeminiAsistanBackend.Application.Interfaces.Repositories;
using GeminiAsistanBackend.Application.Interfaces.SesTetikleyici;
using GeminiAsistanBackend.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Infrastructure.Services;

public class SesTetikleyiciService : ISesTetikleyiciService
{
    public readonly AppDbContext _context;

    public SesTetikleyiciService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SesTetikleyiciResponse> CreateSesTetikleyici(CreateSesTetikleyiciRequest request, CancellationToken cancellationToken)
    {
        // burada id değerine nasıl bakıcaz

        var entites = new Domain.Entities.SesTetikleyicisi
        {
            TetikleyiciMetin = request.TetikleyiciMetin,
            EklenmeTuru = request.EklenmeTuru
        };

        await _context.SesTetikleyicileri.AddAsync(entites,cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return MapToResponse(entites);
    }

    public async Task<List<SesTetikleyiciResponse>> GetAll(CancellationToken cancellationToken)
    {
        var entities = await _context.SesTetikleyicileri
            .AsNoTracking()
            .OrderByDescending(x => x.Id)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToResponse).ToList();
    }

    public async Task<bool> CountSesTetikleyicileri(CancellationToken cancellationToken)
    {
        // bu method eğer ki ses tetikleyicileri içerisinde eklenmeturu = REDMINE olan kayıtlar 10'u geçtiyse kontrol edecek.
       var count = await _context.SesTetikleyicileri
            .AsNoTracking()
            .CountAsync(x => x.EklenmeTuru == EklenmeTuru.REDMINE, cancellationToken);

        if (count >= 15)
            return true;
        return false;
    }

    public static SesTetikleyiciResponse MapToResponse(Domain.Entities.SesTetikleyicisi x)
    {
        return new SesTetikleyiciResponse
        {
            Id = x.Id,
            TetikleyiciMetin = x.TetikleyiciMetin,
            EklenmeTuru = x.EklenmeTuru
        };
    }

}
