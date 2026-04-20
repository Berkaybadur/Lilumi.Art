using Lilumi.Art.Domain.Entities;
using Lilumi.Art.Domain.Interfaces;
using MongoDB.Driver;

namespace Lilumi.Art.Infrastructure.Persistence.Repositories;

public class ProductInquiryRepository(IMongoDatabase database) : IProductInquiryRepository
{
    private readonly IMongoCollection<ProductInquiry> _collection = database.GetCollection<ProductInquiry>("UrunTalepFormlari");

    public async Task<IReadOnlyList<ProductInquiry>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _collection.Find(_ => true).ToListAsync(cancellationToken);

    public async Task<ProductInquiry?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);

    public Task AddAsync(ProductInquiry entity, CancellationToken cancellationToken = default)
        => _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);

    public void Update(ProductInquiry entity)
        => _collection.ReplaceOne(x => x.Id == entity.Id, entity);

    public void Remove(ProductInquiry entity)
        => _collection.DeleteOne(x => x.Id == entity.Id);
}
