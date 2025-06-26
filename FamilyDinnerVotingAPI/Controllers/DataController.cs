using FamilyDinnerVotingAPI.Models.Entities;
using FamilyDinnerVotingAPI.Repositories.Interfaces;
using FamilyDinnerVotingAPI.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FamilyDinnerVotingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DataController : ControllerBase
    {
        private readonly DataSeeder _dataSeeder;
        private readonly IMealRepository _mealRepository;
        private readonly IVoteSessionRepository _voteSessionRepository;
        private readonly IMealVoteSessionRepository _mealVoteSessionRepository;
        private readonly IVoteRepository _voteRepository;
        private readonly ILogger<DataController> _logger;

        public DataController(
            DataSeeder dataSeeder,
            IMealRepository mealRepository,
            IVoteSessionRepository voteSessionRepository,
            IMealVoteSessionRepository mealVoteSessionRepository,
            IVoteRepository voteRepository,
            ILogger<DataController> logger)
        {
            _dataSeeder = dataSeeder;
            _mealRepository = mealRepository;
            _voteSessionRepository = voteSessionRepository;
            _mealVoteSessionRepository = mealVoteSessionRepository;
            _voteRepository = voteRepository;
            _logger = logger;
        }

        // POST: api/data/seed
        [HttpPost("seed")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SeedData()
        {
            try
            {
                await _dataSeeder.SeedAllDataAsync();
                return Ok(new { message = "Data seeded successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding data");
                return StatusCode(500, new { error = "Error seeding data", details = ex.Message });
            }
        }

        // GET: api/data/summary
        [HttpGet("summary")]
        public async Task<IActionResult> GetDataSummary()
        {
            try
            {
                var meals = await _mealRepository.GetAllAsync();
                var voteSessions = await _voteSessionRepository.GetAllAsync();
                var mealVoteSessions = await _mealVoteSessionRepository.GetAllAsync();
                var votes = await _voteRepository.GetAllAsync();

                return Ok(new
                {
                    TotalMeals = meals.Count(),
                    TotalVoteSessions = voteSessions.Count(),
                    TotalMealVoteSessionRelationships = mealVoteSessions.Count(),
                    TotalVotes = votes.Count(),
                    MealsByCategory = meals.GroupBy(m => m.Category)
                        .Select(g => new { Category = g.Key, Count = g.Count() })
                        .ToList(),
                    VoteSessionsByStatus = voteSessions.GroupBy(vs => vs.Status)
                        .Select(g => new { Status = g.Key, Count = g.Count() })
                        .ToList()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting data summary");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/data/meals-with-votes
        [HttpGet("meals-with-votes")]
        public async Task<IActionResult> GetMealsWithVotes()
        {
            try
            {
                var meals = await _mealRepository.GetAllAsync();
                var votes = await _voteRepository.GetAllAsync();

                var mealsWithVoteCounts = meals.Select(meal => new
                {
                    meal.Id,
                    meal.Name,
                    meal.Description,
                    meal.Category,
                    VoteCount = votes.Count(v => v.MealId == meal.Id),
                    LastVoted = votes.Where(v => v.MealId == meal.Id)
                        .OrderByDescending(v => v.TimeStamp)
                        .Select(v => v.TimeStamp)
                        .FirstOrDefault()
                }).OrderByDescending(m => m.VoteCount);

                return Ok(mealsWithVoteCounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting meals with votes");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/data/vote-sessions-with-meals
        [HttpGet("vote-sessions-with-meals")]
        public async Task<IActionResult> GetVoteSessionsWithMeals()
        {
            try
            {
                var voteSessions = await _voteSessionRepository.GetAllAsync();
                var mealVoteSessions = await _mealVoteSessionRepository.GetAllAsync();
                var meals = await _mealRepository.GetAllAsync();

                var voteSessionsWithMeals = voteSessions.Select(session => new
                {
                    session.Id,
                    session.Name,
                    session.StartTime,
                    session.EndTime,
                    session.Status,
                    session.CreatedByUserId,
                    Meals = mealVoteSessions
                        .Where(mvs => mvs.VoteSessionId == session.Id)
                        .Join(meals, mvs => mvs.MealId, meal => meal.Id, (mvs, meal) => new
                        {
                            meal.Id,
                            meal.Name,
                            meal.Category
                        }).ToList()
                });

                return Ok(voteSessionsWithMeals);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vote sessions with meals");
                return StatusCode(500, "Internal server error");
            }
        }
    }
} 