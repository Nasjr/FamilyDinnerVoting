namespace FamilyDinnerVotingAPI.DTOs
{
    public class AddMealsToVoteSessionDto
    {
        public Guid VoteSessionId { get; set; }
        public List<Guid> MealIds { get; set; } = new List<Guid>();
    }
}
