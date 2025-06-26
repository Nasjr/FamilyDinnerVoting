using FamilyDinnerVotingAPI.Data;
using FamilyDinnerVotingAPI.Models.Entities;
using FamilyDinnerVotingAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FamilyDinnerVotingAPI.Services.Implementations
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMealRepository _mealRepository;
        private readonly IVoteSessionRepository _voteSessionRepository;
        private readonly IMealVoteSessionRepository _mealVoteSessionRepository;
        private readonly IVoteRepository _voteRepository;
        private readonly ILogger<DataSeeder> _logger;

        public DataSeeder(
            ApplicationDbContext context,
            UserManager<AppUser> userManager,
            IMealRepository mealRepository,
            IVoteSessionRepository voteSessionRepository,
            IMealVoteSessionRepository mealVoteSessionRepository,
            IVoteRepository voteRepository,
            ILogger<DataSeeder> logger)
        {
            _context = context;
            _userManager = userManager;
            _mealRepository = mealRepository;
            _voteSessionRepository = voteSessionRepository;
            _mealVoteSessionRepository = mealVoteSessionRepository;
            _voteRepository = voteRepository;
            _logger = logger;
        }

        public async Task SeedAllDataAsync()
        {
            try
            {
                _logger.LogInformation("Starting data seeding...");

                // Seed meals
                var meals = await SeedMealsAsync();

                _logger.LogInformation("Starting data seeding For Users...");
                // Seed users (if not already seeded)
                var users = await SeedUsersAsync();
                _logger.LogInformation("Data seeding completed successfully! for Users");


                // Seed vote sessions
                var voteSessions = await SeedVoteSessionsAsync(users);

                // Create relationships between meals and vote sessions
                await SeedMealVoteSessionRelationshipsAsync(meals, voteSessions);

                // Seed some votes
                await SeedVotesAsync(meals, voteSessions, users);

                _logger.LogInformation("Data seeding completed successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during data seeding");
                throw;
            }
        }

        private async Task<List<Meal>> SeedMealsAsync()
        {
            var existingMeals = await _mealRepository.GetAllAsync();
            if (existingMeals.Any())
            {
                _logger.LogInformation("Meals already exist, skipping meal seeding");
                return existingMeals.ToList();
            }

            var meals = new List<Meal>
            {
                new Meal
                {
                    Id = Guid.NewGuid(),
                    Name = "Spaghetti Carbonara",
                    Description = "Classic Italian pasta with eggs, cheese, pancetta, and black pepper",
                    Category = "Italian",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Meal
                {
                    Id = Guid.NewGuid(),
                    Name = "Chicken Tikka Masala",
                    Description = "Creamy and spicy Indian curry with tender chicken",
                    Category = "Indian",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Meal
                {
                    Id = Guid.NewGuid(),
                    Name = "Beef Tacos",
                    Description = "Mexican tacos with seasoned ground beef, lettuce, and cheese",
                    Category = "Mexican",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Meal
                {
                    Id = Guid.NewGuid(),
                    Name = "Grilled Salmon",
                    Description = "Fresh salmon fillet grilled with herbs and lemon",
                    Category = "Seafood",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Meal
                {
                    Id = Guid.NewGuid(),
                    Name = "Caesar Salad",
                    Description = "Fresh romaine lettuce with Caesar dressing, croutons, and parmesan",
                    Category = "Salad",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Meal
                {
                    Id = Guid.NewGuid(),
                    Name = "Margherita Pizza",
                    Description = "Traditional Italian pizza with tomato sauce, mozzarella, and basil",
                    Category = "Italian",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Meal
                {
                    Id = Guid.NewGuid(),
                    Name = "Pad Thai",
                    Description = "Thai stir-fried rice noodles with eggs, tofu, and peanuts",
                    Category = "Thai",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Meal
                {
                    Id = Guid.NewGuid(),
                    Name = "Beef Stir Fry",
                    Description = "Chinese-style beef with vegetables in savory sauce",
                    Category = "Chinese",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Meal
                {
                    Id = Guid.NewGuid(),
                    Name = "Greek Salad",
                    Description = "Fresh vegetables with feta cheese, olives, and olive oil",
                    Category = "Salad",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Meal
                {
                    Id = Guid.NewGuid(),
                    Name = "Chicken Parmesan",
                    Description = "Breaded chicken cutlet with marinara sauce and melted cheese",
                    Category = "Italian",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            foreach (var meal in meals)
            {
                await _mealRepository.AddAsync(meal);
            }
            await _mealRepository.SaveChangesAsync();

            _logger.LogInformation("Seeded {Count} meals", meals.Count);
            return meals;
        }

        private async Task<List<AppUser>> SeedUsersAsync()
        {
            var users = new List<AppUser>();
            var existingUsers = await _userManager.GetUsersInRoleAsync("User");
                // Create some test users
           var testUsers = new[]
           {
               new { Email = "john@family.com", UserName = "john", FullName = "John Smith" },
               new { Email = "sarah@family.com", UserName = "sarah", FullName = "Sarah Johnson" },
               new { Email = "mike@family.com", UserName = "mike", FullName = "Mike Wilson" },
               new { Email = "emma@family.com", UserName = "emma", FullName = "Emma Davis" }
           };

           foreach (var userInfo in testUsers)
           {
               var user = new AppUser
               {
                   UserName = userInfo.UserName,
                   Email = userInfo.Email,
                   FullName = userInfo.FullName,
                   EmailConfirmed = true
               };
               var existingUser = await _userManager.FindByEmailAsync(user.Email);
               if (existingUser != null)
               {
                   _logger.LogInformation("User {Email} already exists, skipping creation", user.Email);
                   continue;
               }

               var result = await _userManager.CreateAsync(user, "Password123!");
               if (result.Succeeded)
               {
                   await _userManager.AddToRoleAsync(user, "User");
                   users.Add(user);
               }
           }
            
       
            _logger.LogInformation("Using {Count} users for seeding", users.Count);
            return users;
        }

        private async Task<List<VoteSession>> SeedVoteSessionsAsync(List<AppUser> users)
        {
            var existingSessions = await _voteSessionRepository.GetAllAsync();
            if (existingSessions.Any())
            {
                _logger.LogInformation("Vote sessions already exist, skipping vote session seeding");
                return existingSessions.ToList();
            }

            var voteSessions = new List<VoteSession>
            {
                new VoteSession
                {
                    Id = Guid.NewGuid(),
                    Name = "Weekend Family Dinner",
                    StartTime = DateTime.UtcNow.AddDays(1),
                    EndTime = DateTime.UtcNow.AddDays(1).AddHours(2),
                    CreatedByUserId = users.First().Id,
                    Status = "Active"
                },
                new VoteSession
                {
                    Id = Guid.NewGuid(),
                    Name = "Friday Night Special",
                    StartTime = DateTime.UtcNow.AddDays(3),
                    EndTime = DateTime.UtcNow.AddDays(3).AddHours(3),
                    CreatedByUserId = users.First().Id,
                    Status = "Active"
                },
                new VoteSession
                {
                    Id = Guid.NewGuid(),
                    Name = "Sunday Brunch",
                    StartTime = DateTime.UtcNow.AddDays(5),
                    EndTime = DateTime.UtcNow.AddDays(5).AddHours(4),
                    CreatedByUserId = users.First().Id,
                    Status = "Active"
                }
            };

            foreach (var session in voteSessions)
            {
                await _voteSessionRepository.AddAsync(session);
            }
            await _voteSessionRepository.SaveChangesAsync();

            _logger.LogInformation("Seeded {Count} vote sessions", voteSessions.Count);
            return voteSessions;
        }

        private async Task SeedMealVoteSessionRelationshipsAsync(List<Meal> meals, List<VoteSession> voteSessions)
        {
            var existingRelationships = await _mealVoteSessionRepository.GetAllAsync();
            if (existingRelationships.Any())
            {
                _logger.LogInformation("Meal-VoteSession relationships already exist, skipping");
                return;
            }

            var random = new Random();
            var relationships = new List<MealVoteSession>();

            foreach (var session in voteSessions)
            {
                // Add 3-5 random meals to each session
                var sessionMeals = meals.OrderBy(x => random.Next()).Take(random.Next(3, 6)).ToList();
                
                foreach (var meal in sessionMeals)
                {
                    var relationship = new MealVoteSession
                    {
                        MealVoteSessionId = Guid.NewGuid(),
                        MealId = meal.Id,
                        VoteSessionId = session.Id
                    };
                    relationships.Add(relationship);
                }
            }

            foreach (var relationship in relationships)
            {
                await _mealVoteSessionRepository.AddAsync(relationship);
            }
            await _mealVoteSessionRepository.SaveChangesAsync();

            _logger.LogInformation("Created {Count} meal-vote session relationships", relationships.Count);
        }

        private async Task SeedVotesAsync(List<Meal> meals, List<VoteSession> voteSessions, List<AppUser> users)
        {
            var existingVotes = await _voteRepository.GetAllAsync();
            if (existingVotes.Any())
            {
                _logger.LogInformation("Votes already exist, skipping vote seeding");
                return;
            }

            var random = new Random();
            var votes = new List<Vote>();

            foreach (var session in voteSessions)
            {
                // Get meals for this session
                var sessionMeals = await _mealVoteSessionRepository.GetMealsByVoteSessionIdAsync(session.Id);
                
                foreach (var user in users)
                {
                    // Each user votes for 1-2 meals in each session
                    var userVotes = sessionMeals.OrderBy(x => random.Next()).Take(random.Next(1, 3)).ToList();
                    
                    foreach (var meal in userVotes)
                    {
                        var vote = new Vote
                        {
                            Id = Guid.NewGuid(),
                            MealId = meal.Id,
                            VoteSessionId = session.Id,
                            UserId = user.Id,
                            TimeStamp = DateTime.UtcNow.AddMinutes(-random.Next(1, 60))
                        };
                        votes.Add(vote);
                    }
                }
            }

            foreach (var vote in votes)
            {
                await _voteRepository.AddAsync(vote);
            }
            await _voteRepository.SaveChangesAsync();

            _logger.LogInformation("Seeded {Count} votes", votes.Count);
        }
    }
} 