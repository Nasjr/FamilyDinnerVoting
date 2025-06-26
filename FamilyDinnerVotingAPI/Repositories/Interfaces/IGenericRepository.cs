using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FamilyDinnerVotingAPI.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = ""
        );
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        Task<T> GetByIdAsync(
            Guid id,
            string includeProperties = ""
        );
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        Task SaveChangesAsync();
        Task<T> AddAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task<T> DeleteAsync(Guid id);
    }
    
    
}
