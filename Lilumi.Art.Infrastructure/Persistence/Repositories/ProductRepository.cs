using Lilumi.Art.Domain.Entities;
using Lilumi.Art.Domain.Interfaces;
using MongoDB.Driver;

namespace Lilumi.Art.Infrastructure.Persistence.Repositories;

public class ProductRepository(IMongoDatabase database) : IProductRepository
{
    private readonly IMongoCollection<Product> _collection = database.GetCollection<Product>("Urunler");

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _collection.Find(_ => true).ToListAsync(cancellationToken);

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);

    public Task AddAsync(Product entity, CancellationToken cancellationToken = default)
        => _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);

    public void Update(Product entity)
        => _collection.ReplaceOne(x => x.Id == entity.Id, entity);

    public void Remove(Product entity)
        => _collection.DeleteOne(x => x.Id == entity.Id);

    public async Task<IReadOnlyList<Product>> GetActiveAsync(CancellationToken cancellationToken = default)
        => await _collection.Find(x => x.IsActive).ToListAsync(cancellationToken);
}
