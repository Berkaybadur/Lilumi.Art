using Lilumi.Art.Application.Contracts.Products;
using System.ComponentModel.DataAnnotations;

namespace Lilumi.Art.Web.Models.Products;

public class ProductDetailViewModel
{
    public ProductDto? Product { get; set; }
    public InquiryFormViewModel InquiryForm { get; set; } = new();
}

public class InquiryFormViewModel
{
    [Required]
    public Guid ProductId { get; set; }

    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    [Required]
    public string Message { get; set; } = string.Empty;
}
