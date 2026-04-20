using System.ComponentModel.DataAnnotations;

namespace Lilumi.Art.Web.Models.Admin;

public class ProductAdminViewModel
{
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Range(1, 100000)]
    public decimal Price { get; set; }

    [Required]
    public string ImageUrl { get; set; } = string.Empty;

    public string LogoUrl { get; set; } = string.Empty;

    [Required]
    public string SourcePlatform { get; set; } = string.Empty;

    [Required]
    public string SourceUrl { get; set; } = string.Empty;

    public string SourceProductUrl { get; set; } = string.Empty;
}
