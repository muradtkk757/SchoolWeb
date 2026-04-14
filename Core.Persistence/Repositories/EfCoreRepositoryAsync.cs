using Core.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.Persistence.Repositories
{
    public class EfCoreRepositoryAsync<T, TContext> : IRepositoryAsync<T, TContext> 
        where T : Entity
        where TContext : DbContext
    {
        private readonly TContext _context;

        public EfCoreRepositoryAsync(TContext context)
        {
            _context = context;
        }

        public virtual async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();  
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            var result = await _context.Set<T>().AsNoTracking().ToListAsync();

            return result;
        }

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            var result = await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate);
           
            return result;
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            var result = await _context.Set<T>().FindAsync(id);
            return result;  
        }

        public async Task<PaginatedResult<T>> GetListAsync(PageRequest pageRequest)
        {
            var query = _context.Set<T>().AsNoTracking();

            var totalCount = await query.CountAsync();
            var items = await query.Skip((pageRequest.PageNumber - 1) * pageRequest.PageSize)
                                   .Take(pageRequest.PageSize)
                                   .ToListAsync();

            var result = new PaginatedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize,
                PageCount = (int)Math.Ceiling(totalCount / (double)pageRequest.PageSize)
            };

            return result;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
