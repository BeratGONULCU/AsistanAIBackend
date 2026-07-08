using GeminiAsistanBackend.Application.DTOs.CihazKomut;
using GeminiAsistanBackend.Application.Interfaces.Repositories;
using GeminiAsistanBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Infrastructure.Repositories;

public class CihazKomutuRepository : GenericRepository<CihazKomutu>, ICihazKomutuRepository
{
    public readonly AppDbContext _context;
    private readonly DbSet<CihazKomutu> _dbset;

    public CihazKomutuRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public Task AddAsync(CihazKomutuResponse entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AnyAsync(Expression<Func<CihazKomutuResponse, bool>> expression, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Delete(CihazKomutuResponse entity)
    {
        throw new NotImplementedException();
    }

    public IQueryable<CihazKomutuResponse> Find(Expression<Func<CihazKomutuResponse, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public async Task<List<CihazKomutu>> GetAllByDomain(string domain, CancellationToken cancellationToken)
    {
        return await _dbSet
            .Where(x => x.domain == domain)
            .ToListAsync();
    }

    public void Update(CihazKomutuResponse entity)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<CihazKomutuResponse>> IGenericRepository<CihazKomutuResponse>.GetAllAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    Task<CihazKomutuResponse?> IGenericRepository<CihazKomutuResponse>.GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    IQueryable<CihazKomutuResponse> IGenericRepository<CihazKomutuResponse>.Query()
    {
        throw new NotImplementedException();
    }
}
