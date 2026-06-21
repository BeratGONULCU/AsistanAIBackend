using GeminiAsistanBackend.Domain.Abstractions;
using GeminiAsistanBackend.Domain.Specifications;

namespace GeminiAsistanBackend.Application.Abstractions.Persistence;

public interface IRepository<T>
    where T : AggregateRoot
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> ListAsync(ISpecification<T>? specification, CancellationToken cancellationToken = default);

    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

    Task RemoveAsync(T entity, CancellationToken cancellationToken = default);
}
