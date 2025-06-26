namespace FamilyDinnerVotingAPI.DTOs
{
    public class CreateVoteSessionDto
    {
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
