using FamilyDinnerVotingAPI.Data;
using FamilyDinnerVotingAPI.DTOs;
using FamilyDinnerVotingAPI.Models.Entities;
using FamilyDinnerVotingAPI.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

public class VoteSessionRepository : GenericRepository<VoteSession>, IVoteSessionRepository
{
    public VoteSessionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<VoteSession> EndVoteSessionAsync(Guid sessionId)
    {
        var session = await GetByIdAsync(sessionId);
        if (session == null || session.Status != "Active")
        {
            return null;
        }
        session.EndTime = DateTime.UtcNow;
        session.Status = "Ended";
        _dbSet.Update(session);
        await _context.SaveChangesAsync();
        return session;
    }

    public async Task<IEnumerable<VoteSession>> GetActiveVoteSessionsAsync()
    {
        var now = DateTime.UtcNow;
        return await _dbSet.Where(vs => vs.Status == "Active" && (vs.EndTime > now))
            .ToListAsync();
    }

    public async Task<VoteSession> StartVoteSessionAsync(Guid sessionId)
    {
        var session = await _dbSet.FindAsync(sessionId);

        if (session == null)
            throw new KeyNotFoundException("Vote session not found.");

        if (session.Status == "Active")
            throw new InvalidOperationException("Session is already active.");

        if (session.StartTime <= DateTime.UtcNow)
            throw new InvalidOperationException("Session has already started.");

        session.StartTime = DateTime.UtcNow;
        session.Status = "Active";

        await _context.SaveChangesAsync();

        return session;
    }

    public async Task<VoteSession> CreateVoteSessionAsync(CreateVoteSessionDto voteSessionDto)
    {
        voteSessionDto.StartTime = DateTime.UtcNow;
        var voteSession = new VoteSession
        {
            Id = Guid.NewGuid(),
            Name = voteSessionDto.Name,
            StartTime = voteSessionDto.StartTime,
            EndTime = voteSessionDto.EndTime,
            Status = "Active",
        };
        await _dbSet.AddAsync(voteSession);
        await _context.SaveChangesAsync();
        return voteSession;
    }
    // Implement any additional methods specific to VoteSessionRepository if needed
    // For example, methods for starting or ending a vote session can be added here
}
