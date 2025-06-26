using FamilyDinnerVotingAPI.DTOs;
using FamilyDinnerVotingAPI.Models.Entities;

namespace FamilyDinnerVotingAPI.Services.Inetrfaces
{
    public interface IVoteSessionService
    {
        Task<VoteSession> StartVoteSessionAsync(Guid sessionId);
        Task<VoteSession> EndVoteSessionAsync(Guid sessionId);
        Task<IEnumerable<VoteSession>> GetActiveVoteSessionsAsync();

        Task<VoteSession> CreateVoteSessionAsync(CreateVoteSessionDto voteSessionDto);
        Task AddMealToVoteSessionAsync(AddMealToVoteSessionDto dto);
        Task AddMealsToVoteSessionAsync(AddMealsToVoteSessionDto dto);



    }
}
