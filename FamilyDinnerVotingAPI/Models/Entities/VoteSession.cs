namespace FamilyDinnerVotingAPI.Models.Entities
{
    public class VoteSession
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string CreatedByUserId { get; set; } // just the ID, no navigation

        public string Status { get; set; }

        // Navigation properties
        public ICollection<Meal> Meals { get; set; } = new List<Meal>();
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
        public ICollection<MealVoteSession> MealVoteSessions { get; set; }



    }
}
