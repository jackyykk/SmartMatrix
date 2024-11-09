using System.Linq.Expressions;

namespace SmartMatrix.Domain.Interfaces.DataAccess.Specifications
{
    public interface ISpec<T>
    {
        Expression<Func<T, bool>>? Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        Expression<Func<T, object>>? OrderBy { get; }
        Expression<Func<T, object>>? OrderByDescending { get; }
    }
}