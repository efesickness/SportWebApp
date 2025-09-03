using Microsoft.AspNetCore.Identity;
using SporWebDeneme1.Entities;
using SporWebDeneme1.Entities.Models;
using System.Text.Json;

public static class DataSeeder
{

    public static async Task SeedPermissionsAndAdminAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        foreach (var perm in DefaultPermissions.All)
        {
            if (!context.Permissions.Any(p => p.Name == perm.Name))
            {
                context.Permissions.Add(perm);
            }
        }
        await context.SaveChangesAsync();

        var adminRole = await roleManager.FindByNameAsync("Admin");
        if (adminRole == null)
        {
            adminRole = new IdentityRole("Admin");
            await roleManager.CreateAsync(adminRole);
        }

        var rolePermissions = context.RolePermissions.Where(rp => rp.RoleId == adminRole.Id).ToList();
        foreach (var perm in context.Permissions)
        {
            if (!rolePermissions.Any(rp => rp.PermissionId == perm.PermissionId))
            {
                context.RolePermissions.Add(new RolePermission
                {
                    RoleId = adminRole.Id,
                    PermissionId = perm.PermissionId
                });
            }
        }

        await context.SaveChangesAsync();
    }

    public static async Task SeedCitiesAndDistrictsAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (!context.Cities.Any())
        {
            var json = await File.ReadAllTextAsync("Data/turkeyCities.json");
            var list = JsonSerializer.Deserialize<List<CitySeed>>(json);

            foreach (var item in list)
            {
                var city = new City { CityName = item.text.Trim() };
                context.Cities.Add(city);
                await context.SaveChangesAsync();

                foreach (var district in item.districts)
                {
                    context.Districts.Add(new District { DistrictName = district.text.Trim(), CityId = city.CityId });
                }
                await context.SaveChangesAsync();
            }
        }
    }
}

public class CitySeed
{
    public int value { get; set; }         
    public string text { get; set; }        
    public List<DistrictSeed> districts { get; set; }
}

public class DistrictSeed
{
    public int value { get; set; }          
    public string text { get; set; }       
}

