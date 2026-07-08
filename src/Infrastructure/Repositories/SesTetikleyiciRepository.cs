using DocumentFormat.OpenXml.Office2010.Excel;
using GeminiAsistanBackend.Application.DTOs.SesTetikleyici;
using GeminiAsistanBackend.Application.Interfaces.Repositories;
using GeminiAsistanBackend.Domain.Entities;
using GeminiAsistanBackend.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Infrastructure.Repositories;

public class SesTetikleyiciRepository : GenericRepository<SesTetikleyicisi>, ISesTetikleyiciRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<SesTetikleyicisi> _dbset;
    private readonly ILogger<SesTetikleyiciRepository> _logger;

    public SesTetikleyiciRepository(AppDbContext context, ILogger<SesTetikleyiciRepository> logger) : base(context)
    {
        _context = context;
        _dbset = _context.Set<SesTetikleyicisi>();
        _logger = logger;   
    }

    public Task AddAsync(SesTetikleyicisi entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> AnyAsync(Expression<Func<SesTetikleyicisi, bool>> expression, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(expression, cancellationToken);
    }

    public Task<SesTetikleyicisi> CompareTetikleyiciMetin(string compareMetin)
    {
        throw new NotImplementedException();
    }

    public void Delete(SesTetikleyicisi entity)
    {
        throw new NotImplementedException();
    }

    public IQueryable<SesTetikleyicisi> Find(Expression<Func<SesTetikleyicisi, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<SesTetikleyiciResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbset.Select(x => new SesTetikleyiciResponse
        {
            Id = x.Id,
            TetikleyiciMetin = x.TetikleyiciMetin,
            EklenmeTuru = x.EklenmeTuru,
            llmConfidenceScore = x.llm_confidence_score, // Entity modelinizdeki isme göre güncelleyin
        }).ToListAsync(cancellationToken);
    }

    public Task<SesTetikleyicisi?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<int> CountTetikleyiciByType(string type,CancellationToken cancellationToken)
    {
        var sayac = await _dbset
            .Where(st => st.TetikleyiciKomutlari.Any(tk =>
            tk.Komut.type.ToLower() == type.ToLower().Trim()
            ))
            .CountAsync(cancellationToken);

        return sayac;
    }

    public async Task<IReadOnlyCollection<SesTetikleyiciResponse>> GetSesTetikleyiciByEklenmeTuru(EklenmeTuru eklenmeturu, CancellationToken cancellationToken)
    {
        var result = await _dbset
            .Where(st => st.EklenmeTuru == eklenmeturu)
            .Select(x => new SesTetikleyiciResponse
            {
                Id = x.Id,
                TetikleyiciMetin = x.TetikleyiciMetin,
                EklenmeTuru = x.EklenmeTuru,
                llmConfidenceScore = x.llm_confidence_score, // Entity modelinizdeki isme göre güncelleyin
            }).ToListAsync(cancellationToken);

        return result.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<SesTetikleyiciResponse>> GetSesTetikleyiciByType(string type, CancellationToken cancellationToken)
    {
        // Hatalı olabilecek GetLabelId metodunu tamamen pas geçiyoruz.
        // Doğrudan veritabanı ilişkisi (Join) üzerinden filtreleme yapıyoruz.
        var result = await _dbset
            .Where(st => st.TetikleyiciKomutlari.Any(tk =>
                // 1. Gelen type değerini küçük harfe çevirip veritabanındakiyle karşılaştırıyoruz
                tk.Komut.type.ToLower() == type.ToLower().Trim()
            ))
            .Select(x => new SesTetikleyiciResponse
            {
                Id = x.Id,
                TetikleyiciMetin = x.TetikleyiciMetin,
                EklenmeTuru = x.EklenmeTuru,
                llmConfidenceScore = x.llm_confidence_score, // Entity modelinizdeki isme göre güncelleyin
            })
            .ToListAsync();

        return result.AsReadOnly();
    }

    public IQueryable<SesTetikleyicisi> Query()
    {
        throw new NotImplementedException();
    }

    public void Update(SesTetikleyicisi entity)
    {
        throw new NotImplementedException();
    }
}
