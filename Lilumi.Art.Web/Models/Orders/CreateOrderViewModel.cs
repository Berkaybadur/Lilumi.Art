using System.ComponentModel.DataAnnotations;

namespace Lilumi.Art.Web.Models.Orders;

public class CreateOrderViewModel
{
    [Required]
    public string ShippingAddress { get; set; } = string.Empty;

    [Required]
    public Guid ProductId { get; set; }

    [Range(1, 20)]
    public int Quantity { get; set; } = 1;
}
