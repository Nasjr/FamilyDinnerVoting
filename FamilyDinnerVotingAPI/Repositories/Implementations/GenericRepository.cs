using FamilyDinnerVotingAPI.Data;
using FamilyDinnerVotingAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FamilyDinnerVotingAPI.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<T> DeleteAsync(Guid id)
        {
          var entity =  await GetByIdAsync(id);
            if (entity == null)
            {
                return null;
            }
            _dbSet.Remove(entity);
            return entity;
        }

        public async Task<bool> ExistsAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<IEnumerable<T>> GetAllAsync
            (System.Linq.Expressions.Expression<Func<T, bool>> filter = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
            string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty.Trim());
                }

            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return await  query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id, string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;
            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty.Trim());
                }
            }
            var entity = await query.FirstOrDefaultAsync(e =>EF.Property<object>(e, "Id").Equals(id));
            return entity;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return entity;
        }
    }
}
