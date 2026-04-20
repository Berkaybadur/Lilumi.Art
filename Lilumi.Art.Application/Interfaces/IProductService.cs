using Lilumi.Art.Application.Contracts.Products;

namespace Lilumi.Art.Application.Interfaces;

public interface IProductService
{
    Task<IReadOnlyList<ProductDto>> GetCatalogAsync(CancellationToken cancellationToken = default);
    Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(ProductDto product, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(ProductDto product, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> CreateInquiryAsync(ProductInquiryRequest request, CancellationToken cancellationToken = default);
}
