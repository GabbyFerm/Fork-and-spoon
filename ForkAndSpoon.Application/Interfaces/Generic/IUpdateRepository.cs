using ForkAndSpoon.Domain.Models;

namespace ForkAndSpoon.Application.Interfaces.Generic
{
    // Handles update operations for any entity type
    public interface IUpdateRepository<T> where T : class
    {
        // Updates an existing entity and returns the result
        Task<OperationResult<T>> UpdateAsync(T entity);
    }
}
