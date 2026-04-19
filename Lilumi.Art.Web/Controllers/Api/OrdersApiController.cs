using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Lilumi.Art.Application.Contracts.Orders;
using Lilumi.Art.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lilumi.Art.Web.Controllers.Api;

[ApiController]
[Route("api/orders")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class OrdersApiController(IOrderService orderService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var result = await orderService.CreateAsync(userId, request, cancellationToken);
        return Ok(result);
    }
}
