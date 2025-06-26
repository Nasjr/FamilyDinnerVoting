using FamilyDinnerVotingAPI.Auth;
using FamilyDinnerVotingAPI.DTOs;
using FamilyDinnerVotingAPI.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;

namespace FamilyDinnerVotingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly IAuthUtils _authUtils;

        public AuthController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config, IAuthUtils authUtils)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
            _authUtils = authUtils;
        }

        // POST /api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new AppUser
            {
                UserName = requestDto.UserName,
                Email = requestDto.Email,
                FullName = requestDto.UserName
            };
            var result = await _userManager.CreateAsync(user, requestDto.Password);
            if (result.Succeeded)
            {
                // Optionally, assign a default role to the user
                if (!await _roleManager.RoleExistsAsync("User"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                }
                await _userManager.AddToRoleAsync(user, "User");
                return Ok(new { message = "User registered successfully." });
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            
             var token = await _authUtils.IssueJwtToken(loginDto.Email, loginDto.Email, loginDto.Password, loginDto.Email, _config);
            if (token == null)
                return NotFound("Wrong Email or Password");
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        [HttpGet("debug/user-info")]
        [Authorize]
        public async Task<IActionResult> GetUserInfo()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                UserId = userId,
                UserName = user.UserName,
                Email = user.Email,
                Roles = roles,
                Claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList()
            });
        }

        [HttpGet("test-auth")]
        [Authorize]
        public IActionResult TestAuth()
        {
            return Ok(new
            {
                Message = "Authentication successful!",
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                UserName = User.FindFirstValue(ClaimTypes.Name),
                Email = User.FindFirstValue(JwtRegisteredClaimNames.Email),
                Roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList(),
                AllClaims = User.Claims.Select(c => new { c.Type, c.Value }).ToList()
            });
        }

  

        [HttpPost("validate-token")]
        public IActionResult ValidateToken([FromBody] string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);
                
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidAudience = _config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                
                return Ok(new
                {
                    IsValid = true,
                    Claims = principal.Claims.Select(c => new { c.Type, c.Value }).ToList(),
                    TokenExpiration = ((JwtSecurityToken)validatedToken).ValidTo
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    IsValid = false,
                    Error = ex.Message,
                    ErrorType = ex.GetType().Name
                });
            }
        }


        [HttpPost("assign-admin/{email}")]
        public async Task<IActionResult> AssignAdminRole(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound("User not found");

            // Ensure Admin role exists
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            var result = await _userManager.AddToRoleAsync(user, "Admin");

            if (result.Succeeded)
            {
                return Ok(new { message = $"Admin role assigned to {email}" });
            }

            return BadRequest(result.Errors);
        }
    }
}
