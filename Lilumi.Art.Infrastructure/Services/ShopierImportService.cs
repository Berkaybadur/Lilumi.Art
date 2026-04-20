using System.Globalization;
using HtmlAgilityPack;
using Lilumi.Art.Application.Interfaces;
using Lilumi.Art.Domain.Entities;
using MongoDB.Driver;

namespace Lilumi.Art.Infrastructure.Services;

public class ShopierImportService(
    IMongoDatabase database,
    IHttpClientFactory httpClientFactory) : IShopierImportService
{
    private const string StoreUrl = "https://www.shopier.com/lilumiart";
    private readonly IMongoCollection<Product> _products = database.GetCollection<Product>("Urunler");

    public async Task<int> ImportAsync(CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient(nameof(ShopierImportService));
        using var request = new HttpRequestMessage(HttpMethod.Get, StoreUrl);
        request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0");

        using var response = await client.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var html = await response.Content.ReadAsStringAsync(cancellationToken);
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var logoUrl = doc.DocumentNode.SelectSingleNode("//meta[@property='og:image']")?.GetAttributeValue("content", string.Empty) ?? string.Empty;
        var cards = doc.DocumentNode.SelectNodes("//div[contains(@class,'product-card') and contains(@class,'shopier--product-card')]");
        if (cards is null || cards.Count == 0)
        {
            return 0;
        }

        var imported = 0;
        foreach (var card in cards)
        {
            var linkNode = card.SelectSingleNode(".//a[contains(@class,'shopier-store--store-product-card-link')]");
            var titleNode = card.SelectSingleNode(".//h3[contains(@class,'shopier-store--store-product-card-title')]");
            var imageNode = card.SelectSingleNode(".//img");
            var priceNode = card.SelectSingleNode(".//div[contains(@class,'shopier-store--store-product-card-price-current')]");

            var sourceProductUrl = linkNode?.GetAttributeValue("href", string.Empty) ?? string.Empty;
            var name = HtmlEntity.DeEntitize(titleNode?.InnerText?.Trim() ?? string.Empty);
            var imageUrl = imageNode?.GetAttributeValue("src", string.Empty) ?? string.Empty;
            var rawPrice = priceNode?.GetAttributeValue("data-price", "0") ?? "0";

            if (string.IsNullOrWhiteSpace(sourceProductUrl) || string.IsNullOrWhiteSpace(name))
            {
                continue;
            }

            var product = await _products.Find(x => x.SourceProductUrl == sourceProductUrl).FirstOrDefaultAsync(cancellationToken);
            if (product is null)
            {
                product = new Product();
                imported++;
            }

            product.Name = name;
            product.Description = name;
            product.Price = ParsePrice(rawPrice);
            product.ImageUrl = imageUrl;
            product.LogoUrl = logoUrl;
            product.SourcePlatform = "Shopier";
            product.SourceUrl = StoreUrl;
            product.SourceProductUrl = sourceProductUrl;
            product.IsActive = true;

            await _products.ReplaceOneAsync(
                x => x.SourceProductUrl == sourceProductUrl,
                product,
                new ReplaceOptions { IsUpsert = true },
                cancellationToken);
        }

        return imported;
    }

    private static decimal ParsePrice(string rawPrice)
    {
        var normalized = rawPrice.Replace("TL", string.Empty).Trim();
        normalized = normalized.Replace(".", string.Empty).Replace(",", ".");
        return decimal.TryParse(normalized, NumberStyles.Any, CultureInfo.InvariantCulture, out var result)
            ? result
            : 0m;
    }
}
