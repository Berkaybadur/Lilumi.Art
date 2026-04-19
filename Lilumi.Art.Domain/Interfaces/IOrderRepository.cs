using Lilumi.Art.Domain.Entities;

namespace Lilumi.Art.Domain.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    Task<IReadOnlyList<Order>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
}
