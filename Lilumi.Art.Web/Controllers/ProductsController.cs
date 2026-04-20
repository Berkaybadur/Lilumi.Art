using Lilumi.Art.Application.Interfaces;
using Lilumi.Art.Application.Contracts.Products;
using Lilumi.Art.Web.Models.Products;
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

    [HttpGet]
    public async Task<IActionResult> Detail(Guid id, CancellationToken cancellationToken)
    {
        var product = await productService.GetByIdAsync(id, cancellationToken);
        if (product is null)
        {
            return NotFound();
        }

        return View(new ProductDetailViewModel
        {
            Product = product,
            InquiryForm = new InquiryFormViewModel
            {
                ProductId = product.Id,
                Message = $"{product.Name} urunu hakkinda bilgi almak istiyorum."
            }
        });
    }

    [HttpPost]
    public async Task<IActionResult> Detail(InquiryFormViewModel form, CancellationToken cancellationToken)
    {
        var product = await productService.GetByIdAsync(form.ProductId, cancellationToken);
        if (product is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(new ProductDetailViewModel { Product = product, InquiryForm = form });
        }

        var saved = await productService.CreateInquiryAsync(
            new ProductInquiryRequest(form.ProductId, form.FullName, form.Email, form.Phone, form.Message),
            cancellationToken);

        if (!saved)
        {
            ModelState.AddModelError(string.Empty, "Form kaydedilemedi.");
            return View(new ProductDetailViewModel { Product = product, InquiryForm = form });
        }

        TempData["Success"] = "Talebiniz alindi, en kisa surede donus yapacagiz.";
        return RedirectToAction(nameof(Detail), new { id = form.ProductId });
    }
}
