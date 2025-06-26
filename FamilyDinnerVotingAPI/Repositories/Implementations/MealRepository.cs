using FamilyDinnerVotingAPI.Data;
using FamilyDinnerVotingAPI.DTOs;
using FamilyDinnerVotingAPI.Models.Entities;
using FamilyDinnerVotingAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FamilyDinnerVotingAPI.Repositories.Implementations
{
    public class MealRepository : GenericRepository<Meal>, IMealRepository
    {
        public MealRepository(ApplicationDbContext context) : base(context)
        {
        }


        public async Task<IEnumerable<Meal>> GetMealsByCategoryAsync(string category)
        {
            //return await GetAllAsync(m=>m.Category == category,q=>q.OrderBy(m=>m.Name),includeProperties:"Votes");
            return await _dbSet.Include("Votes").Where(m => m.Category == category).ToListAsync();
        }

        public async Task<IEnumerable<Meal>> GetMealsByNameAsync(string name)
        {
            return await _dbSet.Where(m => m.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                        
                               .ToListAsync();
        }
    }
}
