using FamilyDinnerVotingAPI.Data;
using FamilyDinnerVotingAPI.DTOs;
using FamilyDinnerVotingAPI.Models.Entities;
using FamilyDinnerVotingAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace FamilyDinnerVotingAPI.Repositories.Implementations
{
    public class MealVoteSessionRepository : GenericRepository<MealVoteSession>, IMealVoteSessionRepository
    {
        private readonly ApplicationDbContext _context;

        public MealVoteSessionRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }

        public async Task AddMealToVoteSessionAsync(AddMealToVoteSessionDto addMealToVoteSessionDto)
        {
            var mealVoteSession = new MealVoteSession
            {
                MealId = addMealToVoteSessionDto.MealId,
                VoteSessionId = addMealToVoteSessionDto.VoteSessionId
            };
            _dbSet.Add(mealVoteSession);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(AddMealToVoteSessionDto addMealToVoteSessionDto)
        {
            return await _dbSet.AnyAsync(mvs => mvs.VoteSessionId == addMealToVoteSessionDto.VoteSessionId && mvs.MealId == addMealToVoteSessionDto.MealId
            );
        }

        public async Task<List<Meal>> GetMealsByVoteSessionIdAsync(Guid voteSessionId)
        {
            return await _context.MealVoteSessions
                .Where(mvs => mvs.VoteSessionId == voteSessionId)
                .Include(mvs => mvs.Meal)
                .Select(mvs => mvs.Meal)
                .ToListAsync();
        }
    }
}
