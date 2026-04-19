using Lilumi.Art.Application.Interfaces;
using Lilumi.Art.Application.Services;
using Lilumi.Art.Domain.Interfaces;
using Lilumi.Art.Infrastructure.Identity;
using Lilumi.Art.Infrastructure.Persistence;
using Lilumi.Art.Infrastructure.Persistence.Repositories;
using Lilumi.Art.Infrastructure.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Lilumi.Art.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoSettings = new MongoSettings
        {
            ConnectionString = configuration["MongoSettings:ConnectionString"] ?? "mongodb://mongo:27017",
            DatabaseName = configuration["MongoSettings:DatabaseName"] ?? "LilumiArtDb"
        };
        services.AddSingleton(mongoSettings);
        services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoSettings.ConnectionString));
        services.AddSingleton(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(mongoSettings.DatabaseName));
        services.AddScoped<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAppUserRepository, AppUserRepository>();

        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
