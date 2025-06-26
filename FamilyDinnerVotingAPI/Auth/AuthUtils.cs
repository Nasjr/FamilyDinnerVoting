using FamilyDinnerVotingAPI.DTOs;
using FamilyDinnerVotingAPI.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FamilyDinnerVotingAPI.Auth
{
    public class AuthUtils : IAuthUtils
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public AuthUtils(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
        }

        public async Task<List<Claim>> GenerateAuthClaims(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            return authClaims;
        }

        public async Task<JwtSecurityToken?>  IssueJwtToken(string userId, string userName,string password ,string Email, IConfiguration config)
        {
            var user = await ValidateUser(password, Email);
            if (user == null)
            {
                return null;
            }

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                expires: DateTime.UtcNow.AddHours(1),
                claims: await GenerateAuthClaims(user),
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256)
            );
            return token;

          
        }

       

   
        private async Task<AppUser?> ValidateUser(string password, string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
                return null;
            return user;
        }
    }
}
