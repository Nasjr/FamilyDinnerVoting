using FamilyDinnerVotingAPI.DTOs;
using FamilyDinnerVotingAPI.Models.Entities;

namespace FamilyDinnerVotingAPI.Repositories.Interfaces
{
    public interface IMealRepository : IGenericRepository<Meal>
    {
        Task<IEnumerable<Meal>> GetMealsByCategoryAsync(string category);
        Task<IEnumerable<Meal>> GetMealsByNameAsync(string name);

      

    }
}
