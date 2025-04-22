using ForkAndSpoon.Application.Interfaces.Generic;
using ForkAndSpoon.Domain.Models;
using ForkAndSpoon.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace ForkAndSpoon.Infrastructure.Repositories.Generic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ForkAndSpoonDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ForkAndSpoonDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<OperationResult<List<T>>> GetAllAsync()
        {
            try
            {
                var list = await _dbSet.ToListAsync();
                return OperationResult<List<T>>.Success(list);
            }
            catch (Exception ex)
            {
                return OperationResult<List<T>>.Failure($"Failed to retrieve list: {ex.Message}");
            }
        }
        public async Task<OperationResult<T>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                    return OperationResult<T>.Failure("Entity not found");

                return OperationResult<T>.Success(entity);
            }
            catch (Exception ex)
            {
                return OperationResult<T>.Failure($"Failed to retrieve entity: {ex.Message}");
            }
        }
        public async Task<OperationResult<T>> CreateAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return OperationResult<T>.Success(entity);
            }
            catch (Exception ex)
            {
                return OperationResult<T>.Failure($"Failed to create entity: {ex.Message}");
            }
        }
        public async Task<OperationResult<T>> UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                return OperationResult<T>.Success(entity);
            }
            catch (Exception ex)
            {
                return OperationResult<T>.Failure($"Failed to update entity: {ex.Message}");
            }
        }
        public async Task<OperationResult<bool>> DeleteAsync(T entity)
        {
            try
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                return OperationResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Failure($"Failed to delete entity: {ex.Message}");
            }
        }
    }
}