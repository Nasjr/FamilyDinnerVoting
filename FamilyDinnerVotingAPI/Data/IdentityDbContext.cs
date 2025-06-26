using FamilyDinnerVotingAPI.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FamilyDinnerVotingAPI.Data
{
    public class IdentityAppDbContext : IdentityDbContext<AppUser>
    {
        public IdentityAppDbContext(DbContextOptions<IdentityAppDbContext> options)
            : base(options) { }
    override protected void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Ignore unrelated entities if EF is picking them up
            builder.Ignore<MealVoteSession>();
            builder.Ignore<VoteSession>();
            builder.Ignore<Meal>();
            builder.Ignore<Vote>();
        }

    }

    
}
