namespace Lilumi.Art.Application.Contracts.Orders;

public record OrderResultDto(Guid OrderId, decimal TotalAmount, string Status);
