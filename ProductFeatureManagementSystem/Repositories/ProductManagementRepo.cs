using MongoDB.Driver;
using ProductFeatureManagementSystem.Models;

namespace ProductFeatureManagementSystem.Repositories;

public class ProductManagementRepo : IProductManagementRepo
{
    private readonly IMongoCollection<ProductFeature> _productFeatures;

    public ProductManagementRepo(IMongoCollection<ProductFeature> productFeatures)
    {
        _productFeatures = productFeatures;
    }
    
    public async Task<ProductFeature> GetByIdAsync(Guid id)
    {
        return await _productFeatures.Find(f => f.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(ProductFeature feature)
    {
        await _productFeatures.InsertOneAsync(feature);
    }

    public async Task UpdateAsync(Guid id, ProductFeature feature)
    {
        await _productFeatures.ReplaceOneAsync(f => f.Id == id, feature);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _productFeatures.DeleteOneAsync(f => f.Id == id);
    }

    public async Task<IEnumerable<ProductFeature>> GetAllAsync()
    {
        return await _productFeatures.Find(_ => true).ToListAsync();
    }
}