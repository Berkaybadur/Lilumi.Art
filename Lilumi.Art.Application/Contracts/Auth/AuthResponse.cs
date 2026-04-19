namespace Lilumi.Art.Application.Contracts.Auth;

public record AuthResponse(
    bool Success,
    string Message,
    string? AccessToken = null,
    string? RefreshToken = null,
    string? UserId = null,
    IReadOnlyList<string>? Roles = null);
