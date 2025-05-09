using ForkAndSpoon.Domain.Models;

namespace ForkAndSpoon.Application.Interfaces.Generic
{
    // Handles delete operations for any entity type
    public interface IDeleteRepository<T> where T : class
    {
        // Deletes the given entity and returns success/failure
        Task<OperationResult<bool>> DeleteAsync(T entity);
    }
}
