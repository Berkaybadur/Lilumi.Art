namespace Lilumi.Art.Application.Interfaces;

public interface IShopierImportService
{
    Task<int> ImportAsync(CancellationToken cancellationToken = default);
}
