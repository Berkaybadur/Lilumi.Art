using Lilumi.Art.Domain.Entities;
using Lilumi.Art.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Lilumi.Art.Domain.Interfaces;

namespace Lilumi.Art.Infrastructure.Persistence.Seeder;

public static class DataSeeder
{
    public static async Task SeedAsync(
        IAppUserRepository userRepository,
        IProductRepository productRepository,
        IPasswordHasher<AppUser> passwordHasher,
        IUnitOfWork unitOfWork)
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

        var products = await productRepository.GetAllAsync();
        if (products.Count > 0)
        {
            return;
        }

        var seedProducts = new List<Product>
        {
            new Product
            {
                Name = "Moon Vase (3D Print)",
                Description = "Hand-finished decorative vase.",
                Price = 1190m,
                ImageUrl = "https://images.unsplash.com/photo-1612196808214-b40f7c3d7f74?q=80&w=900",
                SourcePlatform = "Instagram",
                SourceUrl = "https://www.instagram.com/lilumi.art?igsh=MXZzdTNyNnJ6Zm9lNw=="
            },
            new Product
            {
                Name = "Minimal Figure Lamp",
                Description = "Desk-size figure lamp with warm tone.",
                Price = 1790m,
                ImageUrl = "https://images.unsplash.com/photo-1519710164239-da123dc03ef4?q=80&w=900",
                SourcePlatform = "Shopier",
                SourceUrl = "https://shopier.com/lilumiart"
            },
            new Product
            {
                Name = "Wave Organizer Set",
                Description = "3-piece organizer set for office desks.",
                Price = 890m,
                ImageUrl = "https://images.unsplash.com/photo-1493666438817-866a91353ca9?q=80&w=900",
                SourcePlatform = "Shopier",
                SourceUrl = "https://shopier.com/lilumiart"
            }
        };

        foreach (var product in seedProducts)
        {
            await productRepository.AddAsync(product);
        }

        await unitOfWork.SaveChangesAsync();
    }
}
