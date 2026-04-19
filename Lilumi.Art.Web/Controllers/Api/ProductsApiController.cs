using Lilumi.Art.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lilumi.Art.Web.Controllers.Api;

[ApiController]
[Route("api/products")]
public class ProductsApiController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var products = await productService.GetCatalogAsync(cancellationToken);
        return Ok(products);
    }
}
