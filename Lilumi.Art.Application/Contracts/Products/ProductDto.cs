namespace Lilumi.Art.Application.Contracts.Products;

public record ProductDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string ImageUrl,
    string LogoUrl,
    string SourcePlatform,
    string SourceUrl,
    string SourceProductUrl
);
