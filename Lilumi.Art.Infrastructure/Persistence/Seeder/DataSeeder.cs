using Lilumi.Art.Domain.Entities;
using Lilumi.Art.Application.Interfaces;
using Lilumi.Art.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Lilumi.Art.Domain.Interfaces;

namespace Lilumi.Art.Infrastructure.Persistence.Seeder;

public static class DataSeeder
{
    public static async Task SeedAsync(
        IAppUserRepository userRepository,
        IPasswordHasher<AppUser> passwordHasher,
        IShopierImportService shopierImportService)
    {
        const string adminEmail = "admin@lilumi.art";
        var adminUser = await userRepository.GetByEmailAsync(adminEmail);
        if (adminUser is null)
        {
            adminUser = new AppUser
            {
                Email = adminEmail,
                FullName = "Lilumi Admin",
                Roles = ["Admin", "Customer"]
            };
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin123!");
            await userRepository.AddAsync(adminUser);
        }

        try
        {
            await shopierImportService.ImportAsync();
        }
        catch
        {
            // Startup should continue even if external storefront is temporarily unreachable.
        }
    }
}
