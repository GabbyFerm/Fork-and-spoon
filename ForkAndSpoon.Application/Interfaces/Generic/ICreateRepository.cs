using ForkAndSpoon.Domain.Models;

namespace ForkAndSpoon.Application.Interfaces.Generic
{
    // Handles create operations for any entity type
    public interface ICreateRepository<T> where T : class
    {
        // Adds a new entity and returns the result
        Task<OperationResult<T>> CreateAsync(T entity);
    }
}
