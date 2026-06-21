using System.Linq;

namespace GeminiAsistanBackend.Domain.Specifications;

public static class SpecificationEvaluator
{
    public static IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, ISpecification<T>? specification)
    {
        if (specification?.Criteria is not null)
        {
            inputQuery = inputQuery.Where(specification.Criteria);
        }

        return inputQuery;
    }
}
