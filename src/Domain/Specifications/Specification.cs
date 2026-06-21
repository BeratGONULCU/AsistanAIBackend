using System.Linq.Expressions;

namespace GeminiAsistanBackend.Domain.Specifications;

public abstract class Specification<T> : ISpecification<T>
{
    protected Specification(Expression<Func<T, bool>>? criteria = null)
    {
        Criteria = criteria;
    }

    public Expression<Func<T, bool>>? Criteria { get; protected init; }

    public Specification<T> And(Specification<T> specification) =>
        new CombinedSpecification<T>(this, specification, CombineStrategy.And);

    public Specification<T> Or(Specification<T> specification) =>
        new CombinedSpecification<T>(this, specification, CombineStrategy.Or);

    private sealed class CombinedSpecification<TInner> : Specification<TInner>
    {
        public CombinedSpecification(
            Specification<TInner> left,
            Specification<TInner> right,
            CombineStrategy strategy)
            : base(strategy switch
            {
                CombineStrategy.And => left.Criteria is null
                    ? right.Criteria
                    : right.Criteria is null
                        ? left.Criteria
                        : left.Criteria.And(right.Criteria),
                CombineStrategy.Or => left.Criteria is null
                    ? right.Criteria
                    : right.Criteria is null
                        ? left.Criteria
                        : left.Criteria.Or(right.Criteria),
                _ => throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null)
            })
        {
        }
    }

    private enum CombineStrategy
    {
        And,
        Or
    }
}

internal static class ExpressionExtensions
{
    public static Expression<Func<T, bool>> And<T>(
        this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        var parameter = Expression.Parameter(typeof(T));
        var body = Expression.AndAlso(
            Expression.Invoke(left, parameter),
            Expression.Invoke(right, parameter));
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    public static Expression<Func<T, bool>> Or<T>(
        this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        var parameter = Expression.Parameter(typeof(T));
        var body = Expression.OrElse(
            Expression.Invoke(left, parameter),
            Expression.Invoke(right, parameter));
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}
