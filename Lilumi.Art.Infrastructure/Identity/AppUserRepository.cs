using MongoDB.Driver;

namespace Lilumi.Art.Infrastructure.Identity;

public class AppUserRepository(IMongoDatabase database) : IAppUserRepository
{
    private readonly IMongoCollection<AppUser> _collection = database.GetCollection<AppUser>("users");

    public async Task<AppUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await _collection.Find(x => x.Email == email).FirstOrDefaultAsync(cancellationToken);

    public async Task<AppUser?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);

    public Task AddAsync(AppUser user, CancellationToken cancellationToken = default)
        => _collection.InsertOneAsync(user, cancellationToken: cancellationToken);

    public Task UpdateAsync(AppUser user, CancellationToken cancellationToken = default)
        => _collection.ReplaceOneAsync(x => x.Id == user.Id, user, cancellationToken: cancellationToken);
}
