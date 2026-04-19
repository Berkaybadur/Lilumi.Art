namespace Lilumi.Art.Application.Interfaces;

public interface ITokenService
{
    string CreateToken(string userId, string email, IEnumerable<string> roles);
}
