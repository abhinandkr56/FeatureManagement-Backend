using ProductFeatureManagementSystem.Exceptions;
using ProductFeatureManagementSystem.Models;
using ProductFeatureManagementSystem.Repositories;

namespace ProductFeatureManagementSystem.Services;

public class ProductManagementService : IProductManagementService
{
    private readonly IProductManagementRepo _repository;

    public ProductManagementService(IProductManagementRepo repository)
    {
        _repository = repository;
    }
    public async Task<ProductFeature> GetFeatureAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task AddFeatureAsync(ProductFeature feature)
    {
        if (!ValidateTargetCompletionDate(feature))
        {
            throw new FutureDateRequiredException("Target Completion Date Must be a future date");
        }
        if (!ValidateActualCompletionDate(feature))
        {
            throw new FutureDateRequiredException("Actual Completion Date Must be a future date");
        }

        feature.Id = Guid.NewGuid();
        await _repository.CreateAsync(feature);
        
    }

    public async Task UpdateFeatureAsync(Guid id, ProductFeature feature)
    {
        await _repository.UpdateAsync(id, feature);
    }

    public async Task DeleteFeatureAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<ProductFeature>> GetAllFeaturesAsync()
    {
        return await _repository.GetAllAsync();
    }

    private bool ValidateTargetCompletionDate(ProductFeature productFeature)
    {
        if (productFeature.Status == Status.Active && productFeature.TargetCompletionDate == null)
        {
            return false; 
        }
        if (productFeature.Status == Status.Active && productFeature.TargetCompletionDate.HasValue && productFeature.TargetCompletionDate.Value.Date < DateTime.Today)
        {
            return false; 
        }
        return true;
    }
    
    private bool ValidateActualCompletionDate(ProductFeature productFeature)
    {
        if (productFeature.Status == Status.Closed && productFeature.ActualCompletionDate == null)
        {
            return false; 
        }
        if (productFeature.Status == Status.Closed && productFeature.ActualCompletionDate.HasValue && productFeature.ActualCompletionDate.Value.Date < DateTime.Today)
        {
            return false; 
        }

        return true;
    }
}