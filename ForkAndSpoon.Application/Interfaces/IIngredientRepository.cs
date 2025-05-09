using ForkAndSpoon.Application.Interfaces.Generic;
using ForkAndSpoon.Domain.Models;

namespace ForkAndSpoon.Application.Interfaces
{
    public interface IIngredientRepository : IGenericRepository<Ingredient>
    {
        // Fetches an ingredient by name (used to prevent duplicates)
        Task<OperationResult<Ingredient>> GetByNameAsync(string name);
    }
}
