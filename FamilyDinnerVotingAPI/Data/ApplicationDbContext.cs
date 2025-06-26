using FamilyDinnerVotingAPI.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace FamilyDinnerVotingAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
              
        }


        public DbSet<Meal> Meals { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<VoteSession> VoteSessions { get; set; }
        public DbSet<MealVoteSession> MealVoteSessions { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Vote Composite Key
            builder.Entity<Vote>()
                .HasIndex(v => new { v.UserId, v.VoteSessionId })
                .IsUnique();


            builder.Entity<Vote>()
                .HasOne(v => v.Meal)
                .WithMany(m => m.Votes)
                .HasForeignKey(v => v.MealId);


            builder.Entity<MealVoteSession>()
             .HasKey(mvs => new { mvs.MealId, mvs.VoteSessionId });

            builder.Entity<MealVoteSession>()
                .HasOne(mvs => mvs.Meal)
                .WithMany(m => m.MealVoteSessions)
                .HasForeignKey(mvs => mvs.MealId);

            builder.Entity<MealVoteSession>()
                .HasOne(mvs => mvs.VoteSession)
                .WithMany(vs => vs.MealVoteSessions)
                .HasForeignKey(mvs => mvs.VoteSessionId);
        }

    }
}
