using Lilumi.Art.Domain.Entities;

namespace Lilumi.Art.Domain.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<IReadOnlyList<Product>> GetActiveAsync(CancellationToken cancellationToken = default);
}
