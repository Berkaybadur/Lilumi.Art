using System.Security.Claims;
using Lilumi.Art.Application.Contracts.Orders;
using Lilumi.Art.Application.Interfaces;
using Lilumi.Art.Web.Models.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lilumi.Art.Web.Controllers;

[Authorize]
public class OrdersController(IOrderService orderService, IProductService productService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Create(Guid productId, CancellationToken cancellationToken)
    {
        var products = await productService.GetCatalogAsync(cancellationToken);
        ViewBag.Products = products;
        return View(new CreateOrderViewModel { ProductId = productId == Guid.Empty ? products.First().Id : productId });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderViewModel model, CancellationToken cancellationToken)
    {
        var products = await productService.GetCatalogAsync(cancellationToken);
        ViewBag.Products = products;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var result = await orderService.CreateAsync(
            userId,
            new CreateOrderRequest(
                model.ShippingAddress,
                [new OrderItemRequest(model.ProductId, model.Quantity)]),
            cancellationToken);

        TempData["Success"] = $"Siparis alindi. No: {result.OrderId}, Tutar: {result.TotalAmount} TL";
        return RedirectToAction("Index", "Products");
    }
}
