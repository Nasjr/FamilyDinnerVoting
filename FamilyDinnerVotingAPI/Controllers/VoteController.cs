using FamilyDinnerVotingAPI.DTOs;
using FamilyDinnerVotingAPI.Models.Entities;
using FamilyDinnerVotingAPI.Repositories.Interfaces;
using FamilyDinnerVotingAPI.Services.Implementations;
using FamilyDinnerVotingAPI.Services.Inetrfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FamilyDinnerVotingAPI.Controllers
{
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly IVoteService _voteService;

        public VoteController(IVoteService voteService)
        {
            this._voteService = voteService;
        }
 
        // POST /api/vote/cast
        [HttpPost("cast")]
        public async Task<IActionResult> castVote([FromBody] VoteRequestDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User identity not found.");
            }
            // Check if the user has already voted in this session
            var hasVoted = await _voteService.HasUserVotedAsync(requestDto.VoteSessionId, userId);
            if (hasVoted) {
                return Conflict(new { message = "You have already voted in this session." });
            }
            try
            {
                var vote = new Vote
                {
                    UserId = userId,
                    VoteSessionId = requestDto.VoteSessionId,
                    MealId = requestDto.MealId
                };
                var savedVote = await _voteService.CastVoteAsync(vote);
                return CreatedAtAction(nameof(castVote), new { id = savedVote.Id }, savedVote);

            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // GET /api/vote/session/{sessionId}
        [HttpGet("results/{sessionId}")]
        public async Task<IActionResult> GetVoteResults(Guid sessionId)
        {
            try
            {
                var results = await _voteService.GetVoteResultsAsync(sessionId);
                return Ok(results);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }

        }


        // GET /api/vote/session/{sessionId}/hasvoted
        [HttpGet("GetWinnerMeal/{sessionId}")]
        public async Task<IActionResult> GetWinnerMeal(Guid sessionId)
        {
            try
            {
                var results = await _voteService.GetVoteResultsAsync(sessionId);
                if (results == null || !results.Any())
                {
                    return NotFound(new { message = "No votes found for this session." });
                }
                var winner = results.FirstOrDefault(r => r.IsWinner);
                return Ok(winner);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }

        }


        [HttpGet("{sessionId}/hasvoted")]
        [Authorize]
        public async Task<IActionResult> HasUserVoted(Guid sessionId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var hasVoted = await _voteService.HasUserVotedAsync(sessionId, userId);
            return Ok(new { hasVoted });
        }
    }
}
