using Lilumi.Art.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lilumi.Art.Web.Controllers;

public class ProductsController(IProductService productService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var products = await productService.GetCatalogAsync(cancellationToken);
        return View(products);
    }
}
