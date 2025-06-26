namespace FamilyDinnerVotingAPI.DTOs
{
    public class AddMealToVoteSessionDto
    {
        public Guid VoteSessionId { get; set; }
        public Guid MealId { get; set; }
    }

}
