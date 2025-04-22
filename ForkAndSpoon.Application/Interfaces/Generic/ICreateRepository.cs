using ForkAndSpoon.Domain.Models;

namespace ForkAndSpoon.Application.Interfaces.Generic
{
    public interface ICreateRepository<T> where T : class
    {
        Task<OperationResult<T>> CreateAsync(T entity);
    }
}
