using ForkAndSpoon.Application.Interfaces.Generic;
using ForkAndSpoon.Domain.Models;
using ForkAndSpoon.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ForkAndSpoon.Infrastructure.Repositories.Generic
{
    // A generic repository that handles basic CRUD operations for any entity type T
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ForkAndSpoonDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ForkAndSpoonDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>(); // Gets the DbSet for the type T (e.g., Users, Recipes)
        }

        public async Task<OperationResult<List<T>>> GetAllAsync()
        {
            try
            {
                var list = await _dbSet.ToListAsync(); // Retrieve all entities
                return OperationResult<List<T>>.Success(list); // Return success with the list
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return OperationResult<List<T>>.Failure($"Failed to retrieve list: {ex.Message}");
            }
        }
        public async Task<OperationResult<T>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id); // Try to find the entity
                if (entity == null)
                    return OperationResult<T>.Failure("Entity not found"); // Return failure if not found

                return OperationResult<T>.Success(entity); // Return success if found
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return OperationResult<T>.Failure($"Failed to retrieve entity: {ex.Message}");
            }
        }
        public async Task<OperationResult<T>> CreateAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity); // Add the entity to the DbSet
                await _context.SaveChangesAsync(); // Save changes to the database
                return OperationResult<T>.Success(entity);
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return OperationResult<T>.Failure($"Failed to create entity: {ex.Message}");
            }
        }
        public async Task<OperationResult<T>> UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity); // Mark the entity as modified
                await _context.SaveChangesAsync(); // Save changes to the database
                return OperationResult<T>.Success(entity);
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return OperationResult<T>.Failure($"Failed to update entity: {ex.Message}");
            }
        }
        public async Task<OperationResult<bool>> DeleteAsync(T entity)
        {
            try
            {
                _dbSet.Remove(entity); // Remove the entity from the DbSet
                await _context.SaveChangesAsync(); // Save changes to the database
                return OperationResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return OperationResult<bool>.Failure($"Failed to delete entity: {ex.Message}");
            }
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AnyAsync(predicate);
        }
    }
}