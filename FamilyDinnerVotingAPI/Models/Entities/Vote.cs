namespace FamilyDinnerVotingAPI.Models.Entities
{
    public class Vote
    {
        public Guid Id { get; set; }
        public Guid MealId { get; set; }
        public Guid VoteSessionId { get; set; }
        public string UserId { get; set; }
        public DateTime TimeStamp { get; set; }
        public Meal Meal { get; set; }
        public VoteSession VoteSession { get; set; }
      
     }
}
