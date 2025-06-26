using FamilyDinnerVotingAPI.DTOs;
using FamilyDinnerVotingAPI.Models.Entities;

namespace FamilyDinnerVotingAPI.Services.Inetrfaces
{
    public interface IVoteService
    {
        Task<bool> HasUserVotedAsync(Guid sessionId, string userId);
        Task<Vote> CastVoteAsync(Vote vote);
        Task<IEnumerable<Vote>> GetVotesBySessionAsync(Guid sessionId);
        Task<IEnumerable<VoteResultsDto>> GetVoteResultsAsync(Guid voteSessionId);

    }
}
