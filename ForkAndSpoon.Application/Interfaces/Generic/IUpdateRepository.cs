using ForkAndSpoon.Domain.Models;

namespace ForkAndSpoon.Application.Interfaces.Generic
{
    public interface IUpdateRepository<T> where T : class
    {
        Task<OperationResult<T>> UpdateAsync(T entity);
    }
}
