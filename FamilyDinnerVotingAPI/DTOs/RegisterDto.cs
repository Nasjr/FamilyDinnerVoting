using System.ComponentModel.DataAnnotations;

namespace FamilyDinnerVotingAPI.DTOs
{
    public class RegisterDto
    {
        [Required]
    public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        // Optional: allow selecting role at registration
        public string Role { get; set; } = "User"; // default
    }
}
