using GeminiAsistanBackend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using GeminiAsistanBackend.Application.Interfaces.Repositories;
//using GeminiAsistanBackend.Infrastructure.Context;

namespace GeminiAsistanBackend.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public IQueryable<T> Query()
    {
        return _dbSet.AsQueryable();
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync([id],cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken=default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

    public async Task<bool> AnyAsync(Expression<Func<T,bool>> expression,CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(expression,cancellationToken);
    }

    public IQueryable<T> Find(Expression<Func<T, bool>> expression)
    {
        return _dbSet.Where(expression);
    }

    // EF Core'da Update ve Delete metotlarının asenkron (async) karşılığı yoktur. 
    // Çünkü bu metotlar sadece entity state'ini (Modified/Deleted) değiştirir, IO işlemi yapmaz.
    public async Task AddAsync(T entity,CancellationToken cancellationToken=default)
    {
        await _dbSet.AddAsync(entity,cancellationToken);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    // object id kısmı ile id değerinin property'sinin önemi kalkıyor. int,string
    public async Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync([id], cancellationToken);
    }
}