using ForkAndSpoon.Domain.Models;

namespace ForkAndSpoon.Application.Interfaces.Generic
{
    public interface IDeleteRepository<T> where T : class
    {
        Task<OperationResult<bool>> DeleteAsync(T entity);
    }
}
