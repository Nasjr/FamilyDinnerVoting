using FamilyDinnerVotingAPI.Data;
using FamilyDinnerVotingAPI.Models.Entities;
using FamilyDinnerVotingAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FamilyDinnerVotingAPI.Repositories.Implementations
{
    public class VoteRepository : GenericRepository<Vote>, IVoteRepository
    {
        public VoteRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Vote> CastVoteAsync(Vote vote)
        {
            await _dbSet.AddAsync(vote);
            await _context.SaveChangesAsync();
            return vote;
        }

        public async Task<IEnumerable<Vote>> GetVotesBySessionAsync(Guid sessionId)
        {
            return await _dbSet
                .Where(v => v.VoteSessionId == sessionId)
                .Include(v => v.Meal)
                .ToListAsync();
        }

        public async Task<bool> HasUserVotedAsync(Guid sessionId, string userId)
        {
            return await _dbSet
                .AnyAsync(v => v.VoteSessionId == sessionId && v.UserId == userId);
        }


        // Additional methods specific to VoteRepository can be added here
    }
 
}
