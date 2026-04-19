using Lilumi.Art.Application.Contracts.Orders;

namespace Lilumi.Art.Application.Interfaces;

public interface IOrderService
{
    Task<OrderResultDto> CreateAsync(string userId, CreateOrderRequest request, CancellationToken cancellationToken = default);
}
