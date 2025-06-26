using FamilyDinnerVotingAPI.DTOs;
using FamilyDinnerVotingAPI.Models.Entities;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FamilyDinnerVotingAPI.Repositories.Interfaces
{
    public interface IMealVoteSessionRepository : IGenericRepository<MealVoteSession>
    {
        Task<bool> ExistsAsync(AddMealToVoteSessionDto addMealToVoteSessionDto);
        Task AddMealToVoteSessionAsync(AddMealToVoteSessionDto addMealToVoteSessionDto);
        Task<List<Meal>> GetMealsByVoteSessionIdAsync(Guid voteSessionId);
    }
    
    
}
