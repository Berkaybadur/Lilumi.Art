using Lilumi.Art.Application.Contracts.Auth;
using Lilumi.Art.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lilumi.Art.Web.Controllers.Api;

[ApiController]
[Route("api/auth")]
public class AuthApiController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await authService.RegisterAsync(request, cancellationToken);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await authService.LoginAsync(request, cancellationToken);
        return result.Success ? Ok(result) : Unauthorized(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await authService.RefreshTokenAsync(request, cancellationToken);
        return result.Success ? Ok(result) : Unauthorized(result);
    }
}
