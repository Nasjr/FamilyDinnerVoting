using FamilyDinnerVotingAPI.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FamilyDinnerVotingAPI.Auth
{
    public interface IAuthUtils
    {
        Task<JwtSecurityToken?> IssueJwtToken(string userId, string userName, string password, string Email, IConfiguration config);
        Task<List<Claim>> GenerateAuthClaims(AppUser user);
    }
}
