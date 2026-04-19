namespace Lilumi.Art.Domain.Entities;

public class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string SourcePlatform { get; set; } = string.Empty;
    public string SourceUrl { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
