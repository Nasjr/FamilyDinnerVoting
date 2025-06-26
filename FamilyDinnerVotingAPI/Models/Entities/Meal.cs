namespace FamilyDinnerVotingAPI.Models.Entities
{
    public class Meal
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;


        // Navigation properties
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
        public ICollection<VoteSession> VoteSessions { get; set; } = new List<VoteSession>();
        public ICollection<MealVoteSession> MealVoteSessions { get; set; }

    }
}
