using Lilumi.Art.Domain.Interfaces;

namespace Lilumi.Art.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => Task.FromResult(1);
}
