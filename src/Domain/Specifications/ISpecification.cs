using System.Linq.Expressions;

namespace GeminiAsistanBackend.Domain.Specifications;

public interface ISpecification<T>
{
    Expression<Func<T, bool>>? Criteria { get; }
}
