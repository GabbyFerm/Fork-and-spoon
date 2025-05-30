using System.Linq.Expressions;

namespace ForkAndSpoon.Application.Interfaces.Generic
{
    public interface IExistsRepository<T>
    {
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }
}
