using FamilyDinnerVotingAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FamilyDinnerVotingAPI.Repositories.Interfaces
{
    public interface IVoteRepository : IGenericRepository<Vote>
    {
        Task<bool> HasUserVotedAsync(Guid sessionId, string userId);
        Task<Vote> CastVoteAsync(Vote vote);
        Task<IEnumerable<Vote>> GetVotesBySessionAsync(Guid sessionId);

    }
}
