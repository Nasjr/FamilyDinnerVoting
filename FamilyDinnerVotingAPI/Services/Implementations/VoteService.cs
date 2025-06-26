using FamilyDinnerVotingAPI.DTOs;
using FamilyDinnerVotingAPI.Models.Entities;
using FamilyDinnerVotingAPI.Repositories.Interfaces;
using FamilyDinnerVotingAPI.Services.Inetrfaces;

namespace FamilyDinnerVotingAPI.Services.Implementations
{
    public class VoteService : IVoteService
    {
        private readonly IVoteRepository _voteRepository;
        private readonly IVoteSessionRepository _voteSessionRepository;
        private readonly IGenericRepository<MealVoteSession> _mealVoteSessionRepo;

        public VoteService(IVoteRepository voteRepository,
            IVoteSessionRepository voteSessionRepository,
            IGenericRepository<MealVoteSession> mealVoteSessionRepo
            )
        {
            _voteRepository = voteRepository;
            this._voteSessionRepository = voteSessionRepository;
            this._mealVoteSessionRepo = mealVoteSessionRepo;
        }

        public async Task<Vote> CastVoteAsync(Vote vote)
        {
            var session = await _voteSessionRepository.GetByIdAsync(vote.VoteSessionId);
            if (session == null)
                throw new KeyNotFoundException("Vote session not found.");

            // 2. Ensure the session is still active
            if (session.Status != "Active" || (session.EndTime <= DateTime.UtcNow))
                throw new InvalidOperationException("This vote session is no longer active.");

            // 3. Check if user already voted in this session
            var hasUserVoted = await _voteRepository.HasUserVotedAsync(vote.VoteSessionId, vote.UserId);
            if (hasUserVoted)
                throw new InvalidOperationException("User has already voted in this session.");

            // 4. Validate that the meal belongs to this session
            var mealSessionMatch = await _mealVoteSessionRepo.AnyAsync(ms => ms.MealId == vote.MealId && ms.VoteSessionId == vote.VoteSessionId);

            if (!mealSessionMatch)
                throw new InvalidOperationException("Selected meal is not part of this vote session.");

            // 5. Save the vote
            return await _voteRepository.CastVoteAsync(vote);
        }

        public async Task<IEnumerable<VoteResultsDto>> GetVoteResultsAsync(Guid voteSessionId)
        {

            // Get all votes for the meal for a session
            var meals = await _mealVoteSessionRepo.GetAllAsync(ms => ms.VoteSessionId == voteSessionId,includeProperties: "Meal");
            if (meals == null || !meals.Any())
                throw new KeyNotFoundException("No meals found for this vote session.");

            foreach (var item in meals)
            {
                Console.WriteLine("-----------------------------------------");
                // Console.WriteLine(item.Meal.Name);
                Console.WriteLine(item.Meal.Id);
                Console.WriteLine(item.Meal.Category);
                Console.WriteLine(item.Meal.Description);
                Console.WriteLine("-----------------------------------------");
            }

            var votes = await _voteRepository.GetVotesBySessionAsync(voteSessionId);

            var votesByMeal = votes.GroupBy(v => v.MealId).ToDictionary(g => g.Key, g => g.ToList());

            // Step 2: Combine meals with their vote data
            var mealVoteResults = meals.Select(meal => new VoteResultsDto
            {
                MealId = meal.MealId,
                MealName = meal.Meal.Name ?? "Unknown Meal",

                VoteCount = votesByMeal.TryGetValue(meal.MealId, out var votes)
                            ? votes.Count
                            : 0
            }).ToList();




            //var results = votes.GroupBy(v=> v.MealId).Select(
            //    g => new VoteResultsDto
            //{
            //    MealId = g.Key,
            //    MealName = g.First().Meal.Name,
            //    VoteCount = g.Count(),
            //    }).ToList();

            var maxVotes = mealVoteResults.Max(r => r.VoteCount);
            foreach (var result in mealVoteResults)
            {
                result.IsWinner = result.VoteCount == maxVotes;
            }
            return mealVoteResults;

        }

        public Task<IEnumerable<Vote>> GetVotesBySessionAsync(Guid sessionId)
        {
            return _voteRepository.GetVotesBySessionAsync(sessionId);
        }

    
        

        public Task<bool> HasUserVotedAsync(Guid sessionId, string userId)
        {
            return _voteRepository.HasUserVotedAsync(sessionId, userId);
        }
    }
}
