using FamilyDinnerVotingAPI.Models.Entities;
using FamilyDinnerVotingAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FamilyDinnerVotingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MealController : ControllerBase
    {
        private readonly IMealRepository _mealRepository;
        private readonly ILogger<MealController> _logger;

        public MealController(IMealRepository mealRepository, ILogger<MealController> logger)
        {
            _mealRepository = mealRepository;
            _logger = logger;
        }

        // GET: api/meal
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Meal>>> GetMeals()
        {
            try
            {
                var meals = await _mealRepository.GetAllAsync();
                return Ok(meals);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving meals");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/meal/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Meal>> GetMeal(Guid id)
        {
            try
            {
                var meal = await _mealRepository.GetByIdAsync(id);
                if (meal == null)
                {
                    return NotFound();
                }

                return Ok(meal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving meal with id {MealId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/meal/category/{category}
        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<Meal>>> GetMealsByCategory(string category)
        {
            try
            {
                var meals = await _mealRepository.GetMealsByCategoryAsync(category);
                return Ok(meals);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving meals by category {Category}", category);
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/meal
        [HttpPost]
        public async Task<ActionResult<Meal>> CreateMeal([FromBody] Meal meal)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                meal.Id = Guid.NewGuid();
                meal.CreatedAt = DateTime.UtcNow;
                meal.UpdatedAt = DateTime.UtcNow;

                await _mealRepository.AddAsync(meal);
                await _mealRepository.SaveChangesAsync();

                return CreatedAtAction(nameof(GetMeal), new { id = meal.Id }, meal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating meal");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/meal/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMeal(Guid id, [FromBody] Meal meal)
        {
            try
            {
                if (id != meal.Id)
                {
                    return BadRequest();
                }

                var existingMeal = await _mealRepository.GetByIdAsync(id);
                if (existingMeal == null)
                {
                    return NotFound();
                }

                existingMeal.Name = meal.Name;
                existingMeal.Description = meal.Description;
                existingMeal.Category = meal.Category;
                existingMeal.UpdatedAt = DateTime.UtcNow;

                await _mealRepository.UpdateAsync(existingMeal);
                await _mealRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating meal with id {MealId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/meal/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeal(Guid id)
        {
            try
            {
                var meal = await _mealRepository.GetByIdAsync(id);
                if (meal == null)
                {
                    return NotFound();
                }

                await _mealRepository.DeleteAsync(id);
                await _mealRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting meal with id {MealId}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
} 