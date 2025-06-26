namespace FamilyDinnerVotingAPI.DTOs
{
    public class VoteResultsDto
    {
        public Guid MealId { get; set; }
        public string MealName { get; set; }
        public int VoteCount { get; set; }
        public bool IsWinner { get; set; }
    }
}
