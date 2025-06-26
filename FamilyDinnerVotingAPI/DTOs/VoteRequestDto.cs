namespace FamilyDinnerVotingAPI.DTOs
{
    public class VoteRequestDto
    {
        public Guid VoteSessionId { get; set; }
        public Guid MealId { get; set; }
    }
}
