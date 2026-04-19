namespace Lilumi.Art.Application.Contracts.Orders;

public record CreateOrderRequest(string ShippingAddress, List<OrderItemRequest> Items);
