using Lilumi.Art.Application.Contracts.Auth;
using Lilumi.Art.Application.Interfaces;
using Lilumi.Art.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace Lilumi.Art.Infrastructure.Security;

public class AuthService(
    IAppUserRepository userRepository,
    IPasswordHasher<AppUser> passwordHasher,
    ITokenService tokenService) : IAuthService
{
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var exists = await userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (exists is not null)
        {
            return new AuthResponse(false, "User already exists.");
        }

        var user = new AppUser
        {
            Email = request.Email,
            FullName = request.FullName,
            Roles = ["Customer"]
        };
        user.PasswordHash = passwordHasher.HashPassword(user, request.Password);

        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryUtc = DateTime.UtcNow.AddDays(14);
        await userRepository.AddAsync(user, cancellationToken);

        var roles = new[] { "Customer" };
        var token = tokenService.CreateToken(user.Id, user.Email, roles);
        return new AuthResponse(true, "Registration successful.", token, refreshToken, user.Id, roles);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user is null)
        {
            return new AuthResponse(false, "Invalid credentials.");
        }

        var verifyResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (verifyResult == PasswordVerificationResult.Failed)
        {
            return new AuthResponse(false, "Invalid credentials.");
        }

        var roles = user.Roles;
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryUtc = DateTime.UtcNow.AddDays(14);
        await userRepository.UpdateAsync(user, cancellationToken);

        var token = tokenService.CreateToken(user.Id, user.Email, roles);
        return new AuthResponse(true, "Login successful.", token, refreshToken, user.Id, roles.ToList());
    }

    public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryUtc <= DateTime.UtcNow)
        {
            return new AuthResponse(false, "Invalid refresh token.");
        }

        var roles = user.Roles;
        var newAccessToken = tokenService.CreateToken(user.Id, user.Email, roles);
        var newRefreshToken = GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryUtc = DateTime.UtcNow.AddDays(14);
        await userRepository.UpdateAsync(user, cancellationToken);

        return new AuthResponse(true, "Token refreshed.", newAccessToken, newRefreshToken, user.Id, roles.ToList());
    }

    private static string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        RandomNumberGenerator.Fill(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}
