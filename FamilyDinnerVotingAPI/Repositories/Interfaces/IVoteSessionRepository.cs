using FamilyDinnerVotingAPI.DTOs;
using FamilyDinnerVotingAPI.Models.Entities;
using FamilyDinnerVotingAPI.Repositories.Implementations;
using FamilyDinnerVotingAPI.Repositories.Interfaces;

public interface IVoteSessionRepository : IGenericRepository<VoteSession>
{
    // Define any additional methods specific to VoteSession if needed
    // For example, you might want to add methods for starting or ending a vote session
    Task<VoteSession> StartVoteSessionAsync(Guid sessionId);
    Task<VoteSession> EndVoteSessionAsync(Guid sessionId);
    Task<IEnumerable<VoteSession>> GetActiveVoteSessionsAsync();

    Task<VoteSession> CreateVoteSessionAsync(CreateVoteSessionDto voteSessionDto);
}
