namespace Lilumi.Art.Application.Contracts.Products;

public record ProductInquiryRequest(
    Guid ProductId,
    string FullName,
    string Email,
    string Phone,
    string Message
);
