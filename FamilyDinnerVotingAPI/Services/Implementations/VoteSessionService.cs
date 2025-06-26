using FamilyDinnerVotingAPI.DTOs;
using FamilyDinnerVotingAPI.Models.Entities;
using FamilyDinnerVotingAPI.Repositories.Interfaces;
using FamilyDinnerVotingAPI.Services.Inetrfaces;

namespace FamilyDinnerVotingAPI.Services.Implementations
{
    public class VoteSessionService : IVoteSessionService
    {
        private readonly IVoteSessionRepository _voteSessionRepository;
        private readonly IMealRepository _mealRepository;
        private readonly IMealVoteSessionRepository _mealVoteSessionRepository;

        public VoteSessionService(IVoteSessionRepository voteSessionRepository,
            IMealRepository mealRepository,
            IMealVoteSessionRepository mealVoteSessionRepository)
        {
            this._voteSessionRepository = voteSessionRepository;
            this._mealRepository = mealRepository;
            this._mealVoteSessionRepository = mealVoteSessionRepository;
        }

        public async Task AddMealsToVoteSessionAsync(AddMealsToVoteSessionDto dto)
        {
            var session = await _voteSessionRepository.GetByIdAsync(dto.VoteSessionId);
            if (session == null)
                throw new KeyNotFoundException("Vote session not found.");

            foreach (var id in dto.MealIds.Distinct())
            {
                var meal = await _mealRepository.GetByIdAsync(id);
                if (meal == null)
                    throw new KeyNotFoundException($"Meal with ID {id} not found.");

                var exists = await _mealVoteSessionRepository.ExistsAsync(new AddMealToVoteSessionDto { MealId=id,VoteSessionId = dto.VoteSessionId });
                if (!exists)
                {
                    await _mealVoteSessionRepository.AddMealToVoteSessionAsync(new AddMealToVoteSessionDto { MealId = id, VoteSessionId = dto.VoteSessionId });
                }
            }
        }

        public async Task AddMealToVoteSessionAsync(AddMealToVoteSessionDto dto)
        {
            // 1. Check if vote session exists
            var session = await _voteSessionRepository.GetByIdAsync(dto.VoteSessionId);
            if (session == null)
                throw new KeyNotFoundException("Vote session not found.");

            // 2. Check if meal exists
            var meal = await _mealRepository.GetByIdAsync(dto.MealId);
            if (meal == null)
                throw new KeyNotFoundException("Meal not found.");

            // 3. Check if already linked
            var alreadyLinked = await _mealVoteSessionRepository.ExistsAsync(dto);
            if (alreadyLinked)
                throw new InvalidOperationException("Meal is already added to this vote session.");

            // 4. Link meal to session
            await _mealVoteSessionRepository.AddMealToVoteSessionAsync(dto);
        }

        public Task<VoteSession> CreateVoteSessionAsync(CreateVoteSessionDto voteSessionDto)
        {
            return _voteSessionRepository.CreateVoteSessionAsync(voteSessionDto);
        }

        public async Task<VoteSession> EndVoteSessionAsync(Guid sessionId)
        {
            var session = await _voteSessionRepository.GetByIdAsync(sessionId);
            if (session == null)
                throw new KeyNotFoundException("Vote session not found.");
            if (session.Status != "Active")
                throw new InvalidOperationException("Only active vote sessions can be ended.");
            return await _voteSessionRepository.EndVoteSessionAsync(sessionId);
        }

        public async Task<IEnumerable<VoteSession>> GetActiveVoteSessionsAsync()
        {
            var activeSessions = await _voteSessionRepository.GetActiveVoteSessionsAsync();
            if (activeSessions == null || !activeSessions.Any())
                throw new InvalidOperationException("There Are no Current Active Sessions");
            return activeSessions;
        }

        public async Task<VoteSession> StartVoteSessionAsync(Guid sessionId)
        {
            var currentSession = await _voteSessionRepository.GetByIdAsync(sessionId);

            if (currentSession.Status == "Active"){
                throw new InvalidOperationException("There is already an active vote session.");
            }
             
            
            if (currentSession.Status == "Ended")
            {
            throw new InvalidOperationException("This session has already ended.");
            }
            

            

            return await _voteSessionRepository.StartVoteSessionAsync(sessionId);
        }
    }
}
