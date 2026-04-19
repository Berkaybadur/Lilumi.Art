namespace Lilumi.Art.Application.Contracts.Orders;

public record OrderItemRequest(Guid ProductId, int Quantity);
