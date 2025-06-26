// Update your IdentitySeeder.cs

using Microsoft.AspNetCore.Identity;
using FamilyDinnerVotingAPI.Models.Entities;

public class IdentitySeeder
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;

    public IdentitySeeder(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task SeedRolesAsync()
    {
        // Create roles if they don't exist
        string[] roles = { "Admin", "User" };

        foreach (string role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    public async Task SeedAdminUserAsync()
    {
        // Create default admin user
        var adminEmail = "admin@familydinner.com";
        var adminUser = await _userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new AppUser
            {
                UserName = "admin",
                Email = adminEmail,
                FullName = "System Administrator",
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(adminUser, "Admin123!");

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}