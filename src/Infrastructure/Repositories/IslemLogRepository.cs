using GeminiAsistanBackend.Application.DTOs.IslemLog;
using GeminiAsistanBackend.Application.Interfaces.Repositories;
using GeminiAsistanBackend.Domain.Entities;
using GeminiAsistanBackend.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Infrastructure.Repositories;

public class IslemLogRepository : GenericRepository<IslemLog>, IIslemLogRepository
{
    public readonly AppDbContext _context;
    public readonly DbSet<IslemLog> _dbset;
    public IslemLogRepository(AppDbContext context) : base(context)
    {
        _context = context;
        _dbset = _context.Set<IslemLog>();
    }

    public async Task<List<IslemLog?>> GetByMetin(string metin, CancellationToken cancellationToken = default)
    {
        //return await _dbset.FirstOrDefaultAsync(x => x.DuyulanSes.Contains(metin), cancellationToken);
        return await _dbset
            .Where(x => x.DuyulanSes.Contains(metin))
            .ToListAsync(cancellationToken);
    }

    public Task<IslemLog> getLogbyId(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<IslemLog>> getLogs()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<IslemLog>> GetLogsByDate(CancellationToken cancellationToken = default)
    {
        return await _dbset
            .OrderByDescending(x => x.TarihSaat)
            .ToListAsync();
    }

    public async Task<IEnumerable<IslemLog>> GetIslemLogByDurum(string durum, CancellationToken cancellationToken)
    {
        if(!Enum.TryParse<IslemDurum>(durum,true,out var IslemDurum))
        {
            return Enumerable.Empty<IslemLog>();
        }

        return await _dbset
            .Where(x => x.Durum == IslemDurum)
            .OrderBy(x => x.TarihSaat)
            .ToListAsync();
    }

}

