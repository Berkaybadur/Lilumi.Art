using Lilumi.Art.Application.Contracts.Products;
using Lilumi.Art.Application.Interfaces;
using Lilumi.Art.Domain.Entities;
using Lilumi.Art.Domain.Interfaces;

namespace Lilumi.Art.Application.Services;

public class ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork) : IProductService
{
    public async Task<IReadOnlyList<ProductDto>> GetCatalogAsync(CancellationToken cancellationToken = default)
    {
        var products = await productRepository.GetActiveAsync(cancellationToken);
        return products.Select(p => new ProductDto(
            p.Id,
            p.Name,
            p.Description,
            p.Price,
            p.ImageUrl,
            p.SourcePlatform,
            p.SourceUrl)).ToList();
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var p = await productRepository.GetByIdAsync(id, cancellationToken);
        return p is null
            ? null
            : new ProductDto(p.Id, p.Name, p.Description, p.Price, p.ImageUrl, p.SourcePlatform, p.SourceUrl);
    }

    public async Task<Guid> CreateAsync(ProductDto product, CancellationToken cancellationToken = default)
    {
        var entity = new Product
        {
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            SourcePlatform = product.SourcePlatform,
            SourceUrl = product.SourceUrl,
            IsActive = true
        };
        await productRepository.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

    public async Task<bool> UpdateAsync(ProductDto product, CancellationToken cancellationToken = default)
    {
        var entity = await productRepository.GetByIdAsync(product.Id, cancellationToken);
        if (entity is null)
        {
            return false;
        }

        entity.Name = product.Name;
        entity.Description = product.Description;
        entity.Price = product.Price;
        entity.ImageUrl = product.ImageUrl;
        entity.SourcePlatform = product.SourcePlatform;
        entity.SourceUrl = product.SourceUrl;
        productRepository.Update(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await productRepository.GetByIdAsync(id, cancellationToken);
        if (entity is null)
        {
            return false;
        }

        entity.IsActive = false;
        productRepository.Update(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
