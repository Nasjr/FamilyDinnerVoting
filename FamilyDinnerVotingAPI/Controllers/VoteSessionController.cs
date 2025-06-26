using FamilyDinnerVotingAPI.DTOs;
using FamilyDinnerVotingAPI.Models.Entities;
using FamilyDinnerVotingAPI.Services.Inetrfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FamilyDinnerVotingAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class VoteSessionController : ControllerBase
    {
        private readonly IVoteSessionService _voteSessionService;

        public VoteSessionController(IVoteSessionService voteSessionService)
        {
            this._voteSessionService = voteSessionService;
        }

        // POST /api/votesession
        [HttpPost("start/{sessionId}")]
        public async Task<IActionResult> StartVoteSession(Guid sessionId)
        {
            try
            {
                var session = await _voteSessionService.StartVoteSessionAsync(sessionId);
                return Ok(session);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }



        // POST /api/votesession/end/{sessionId}
        [HttpPost("end/{sessionId}")]
        public async Task<IActionResult> EndVoteSession(Guid sessionId)
        {
            try
            {
                var endedSession = await _voteSessionService.EndVoteSessionAsync(sessionId);
                return Ok(endedSession);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        // GET /api/votesession/active
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveVoteSessions()
        {
            try
            {
                var activeSessions = await _voteSessionService.GetActiveVoteSessionsAsync();

                return Ok(activeSessions);
            }
            catch (InvalidOperationException ex)
            {

                return Conflict(ex.Message);
            }
            
        }


        // POST /api/votesession/assign-meals
        [HttpPost("assign-meals")]
        public async Task<IActionResult> AddMealsToVoteSession([FromBody] AddMealsToVoteSessionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _voteSessionService.AddMealsToVoteSessionAsync(dto);
                return Ok("Meals successfully added to vote session.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        // POST /api/votesession/assign-meals
        [HttpPost("assign-meal")]
        public async Task<IActionResult> AddMealToVoteSession([FromBody] AddMealToVoteSessionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _voteSessionService.AddMealToVoteSessionAsync(dto);
                return Ok("Meal successfully added to vote session.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPost("create")]
   
        public async Task<IActionResult> CreateVoteSession([FromBody] CreateVoteSessionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (dto == null)
                return BadRequest("Vote session data is required.");
            if (string.IsNullOrEmpty(dto.Name) || dto.StartTime == default || dto.EndTime == default)
            {
                return BadRequest("Invalid vote session data. Please provide a valid name, start time, and end time.");
            }

            var created = await _voteSessionService.CreateVoteSessionAsync(dto);
            return Ok(created);
        }
    }
}
