using Microsoft.AspNetCore.Identity;
using SporWebDeneme1.Entities.Models;

namespace SporWebDeneme1.Identity
{
    public class IdentityServices
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Admin", "Instructor", "Student" };

            foreach (var roleName in roleNames)
            {
                var roleExists = await roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                    await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            var adminEmail = "msarslan@uludag.edu.tr";
            var adminPassword = "TempAdminPassword123!";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    Address = "Admin Address",
                    PhoneNumber = "1234567890",
                    Name = "Admin",
                    Surname = "Admin",
                    TC_Number = "12345678901",
                    LastLoginDate = DateTime.Now,
                    BirthDate = new DateOnly(1990, 1, 1),
                    CityId = 1,
                    DistrictId = 1,
                    BloodType = "0+",
                    RegistrationDate = DateTime.Now,
                    Gender = "E"
                };
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }

    }
}
