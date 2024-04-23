using ProductFeatureManagementSystem.Models;

namespace ProductFeatureManagementSystem.Services;

public interface IProductManagementService
{
    Task<ProductFeature> GetFeatureAsync(Guid id);
    Task AddFeatureAsync(ProductFeature feature);
    Task UpdateFeatureAsync(Guid id, ProductFeature feature);
    Task DeleteFeatureAsync(Guid id);
    Task<IEnumerable<ProductFeature>> GetAllFeaturesAsync();
}