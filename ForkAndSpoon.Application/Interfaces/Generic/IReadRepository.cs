using ForkAndSpoon.Domain.Models;

namespace ForkAndSpoon.Application.Interfaces.Generic
{
    // Handles read operations for any entity type
    public interface IReadRepository<T> where T : class
    {
        // Gets a single entity by its ID
        Task<OperationResult<T>> GetByIdAsync(int id);

        // Gets all entities of this type
        Task<OperationResult<List<T>>> GetAllAsync();
    }
}
