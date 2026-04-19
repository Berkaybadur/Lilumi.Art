using Lilumi.Art.Domain.Entities;
using Lilumi.Art.Domain.Interfaces;
using MongoDB.Driver;

namespace Lilumi.Art.Infrastructure.Persistence.Repositories;

public class OrderRepository(IMongoDatabase database) : IOrderRepository
{
    private readonly IMongoCollection<Order> _collection = database.GetCollection<Order>("orders");

    public async Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _collection.Find(_ => true).ToListAsync(cancellationToken);

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);

    public Task AddAsync(Order entity, CancellationToken cancellationToken = default)
        => _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);

    public void Update(Order entity)
        => _collection.ReplaceOne(x => x.Id == entity.Id, entity);

    public void Remove(Order entity)
        => _collection.DeleteOne(x => x.Id == entity.Id);

    public async Task<IReadOnlyList<Order>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        => await _collection.Find(o => o.UserId == userId).ToListAsync(cancellationToken);
}
