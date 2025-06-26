namespace FamilyDinnerVotingAPI.Models.Entities
{
    public class MealVoteSession
    {

        public Guid MealVoteSessionId { get; set; }

        public Guid MealId { get; set; }
        public Meal Meal { get; set; }

        public Guid VoteSessionId { get; set; }
        public VoteSession VoteSession { get; set; }
    }
}
