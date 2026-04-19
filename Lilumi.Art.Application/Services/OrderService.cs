using Lilumi.Art.Application.Contracts.Orders;
using Lilumi.Art.Application.Interfaces;
using Lilumi.Art.Domain.Entities;
using Lilumi.Art.Domain.Interfaces;

namespace Lilumi.Art.Application.Services;

public class OrderService(
    IProductRepository productRepository,
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork) : IOrderService
{
    public async Task<OrderResultDto> CreateAsync(string userId, CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        var order = new Order
        {
            UserId = userId,
            ShippingAddress = request.ShippingAddress
        };

        foreach (var item in request.Items.Where(i => i.Quantity > 0))
        {
            var product = await productRepository.GetByIdAsync(item.ProductId, cancellationToken)
                ?? throw new InvalidOperationException("Product not found.");

            order.Items.Add(new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            });
        }

        order.TotalAmount = order.Items.Sum(i => i.UnitPrice * i.Quantity);

        await orderRepository.AddAsync(order, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new OrderResultDto(order.Id, order.TotalAmount, order.Status);
    }
}
