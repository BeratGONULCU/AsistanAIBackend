using System.Linq;
using GeminiAsistanBackend.Application.Abstractions.Persistence;
using GeminiAsistanBackend.Domain.Abstractions;
using GeminiAsistanBackend.Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace GeminiAsistanBackend.Infrastructure.Persistence.Repositories;

public class Repository<T> : IRepository<T>
    where T : AggregateRoot
{
    protected readonly AppDbContext Context;
    private readonly DbSet<T> _set;

    public Repository(AppDbContext context)
    {
        Context = context;
        _set = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _set.FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }

    public virtual async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _set.AsNoTracking().ToListAsync(cancellationToken);
    }

    public virtual async Task<IReadOnlyList<T>> ListAsync(
        ISpecification<T>? specification,
        CancellationToken cancellationToken = default)
    {
        var queryable = ApplySpecification(specification);
        return await queryable.ToListAsync(cancellationToken);
    }

    public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _set.AddAsync(entity, cancellationToken);
    }

    public virtual Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _set.Update(entity);
        return Task.CompletedTask;
    }

    public virtual Task RemoveAsync(T entity, CancellationToken cancellationToken = default)
    {
        _set.Remove(entity);
        return Task.CompletedTask;
    }

    protected IQueryable<T> ApplySpecification(ISpecification<T>? specification)
    {
        var query = _set.AsQueryable();
        return SpecificationEvaluator.GetQuery(query, specification);
    }
}
