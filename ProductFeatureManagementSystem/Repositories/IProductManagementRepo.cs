using ProductFeatureManagementSystem.Models;

namespace ProductFeatureManagementSystem.Repositories;

public interface IProductManagementRepo
{
    Task<ProductFeature> GetByIdAsync(Guid id);
    Task CreateAsync(ProductFeature feature);
    Task UpdateAsync(Guid id, ProductFeature feature);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<ProductFeature>> GetAllAsync();
}