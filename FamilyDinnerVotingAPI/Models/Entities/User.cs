using Microsoft.AspNetCore.Identity;

namespace FamilyDinnerVotingAPI.Models.Entities
{
    

    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }

        public ICollection<Vote> Votes { get; set; }
        public ICollection<VoteSession> CreatedVoteSessions { get; set; }  
         
    }

}
