using Lilumi.Art.Application.Interfaces;
using Lilumi.Art.Application.Services;
using Lilumi.Art.Domain.Interfaces;
using Lilumi.Art.Infrastructure.Identity;
using Lilumi.Art.Infrastructure.Persistence;
using Lilumi.Art.Infrastructure.Persistence.Repositories;
using Lilumi.Art.Infrastructure.Security;
using Lilumi.Art.Infrastructure.Services;
using Lilumi.Art.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Lilumi.Art.Infrastructure;

public static class DependencyInjection
{
    private static bool _guidSerializerRegistered;

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        RegisterMongoGuidSerializers();

        var mongoSettings = new MongoSettings
        {
            ConnectionString = configuration["MongoSettings:ConnectionString"] ?? "mongodb://mongo:27017",
            DatabaseName = configuration["MongoSettings:DatabaseName"] ?? "LilumiArtDb"
        };
        services.AddSingleton(mongoSettings);
        services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoSettings.ConnectionString));
        services.AddSingleton(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(mongoSettings.DatabaseName));
        services.AddScoped<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();
        services.AddHttpClient(nameof(ShopierImportService));

        var smtpSettings = new SmtpSettings
        {
            Host = configuration["Smtp:Host"] ?? "smtp.gmail.com",
            Port = int.TryParse(configuration["Smtp:Port"], out var smtpPort) ? smtpPort : 587,
            Username = configuration["Smtp:Username"] ?? string.Empty,
            Password = configuration["Smtp:Password"] ?? string.Empty,
            FromEmail = configuration["Smtp:FromEmail"] ?? "no-reply@lilumi.art",
            ToEmail = configuration["Smtp:ToEmail"] ?? "admin@lilumi.art",
            EnableSsl = !bool.TryParse(configuration["Smtp:EnableSsl"], out var ssl) || ssl
        };
        services.AddSingleton(smtpSettings);

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductInquiryRepository, ProductInquiryRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAppUserRepository, AppUserRepository>();

        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmailService, SmtpEmailService>();
        services.AddScoped<IShopierImportService, ShopierImportService>();

        return services;
    }

    private static void RegisterMongoGuidSerializers()
    {
        if (_guidSerializerRegistered)
        {
            return;
        }

        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        BsonSerializer.RegisterSerializer(new NullableSerializer<Guid>(new GuidSerializer(GuidRepresentation.Standard)));
        _guidSerializerRegistered = true;
    }
}
