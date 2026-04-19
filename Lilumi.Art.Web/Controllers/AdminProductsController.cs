using Lilumi.Art.Application.Contracts.Products;
using Lilumi.Art.Application.Interfaces;
using Lilumi.Art.Web.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lilumi.Art.Web.Controllers;

[Authorize(Roles = "Admin")]
public class AdminProductsController(IProductService productService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var products = await productService.GetCatalogAsync(cancellationToken);
        return View(products);
    }

    [HttpGet]
    public IActionResult Create() => View(new ProductAdminViewModel());

    [HttpPost]
    public async Task<IActionResult> Create(ProductAdminViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await productService.CreateAsync(Map(model), cancellationToken);
        TempData["Success"] = "Urun olusturuldu.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var product = await productService.GetByIdAsync(id, cancellationToken);
        if (product is null)
        {
            return NotFound();
        }

        return View(new ProductAdminViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            ImageUrl = product.ImageUrl,
            SourcePlatform = product.SourcePlatform,
            SourceUrl = product.SourceUrl
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ProductAdminViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var updated = await productService.UpdateAsync(Map(model), cancellationToken);
        if (!updated)
        {
            return NotFound();
        }

        TempData["Success"] = "Urun guncellendi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await productService.DeleteAsync(id, cancellationToken);
        TempData["Success"] = "Urun pasiflestirildi.";
        return RedirectToAction(nameof(Index));
    }

    private static ProductDto Map(ProductAdminViewModel model)
        => new(model.Id, model.Name, model.Description, model.Price, model.ImageUrl, model.SourcePlatform, model.SourceUrl);
}
