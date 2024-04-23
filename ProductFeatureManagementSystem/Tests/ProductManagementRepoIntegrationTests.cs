using MongoDB.Driver;
using ProductFeatureManagementSystem.Models;
using ProductFeatureManagementSystem.Repositories;
using Xunit;

namespace ProductFeatureManagementSystem.Tests;

public class ProductManagementRepoIntegrationTests
{
    private readonly IMongoCollection<ProductFeature> _productFeatures;
    private readonly ProductManagementRepo _repository;

    public ProductManagementRepoIntegrationTests()
    {
        var client = new MongoClient("mongodb://localhost:27019");
        var database = client.GetDatabase("TestProductDatabase"); 
        _productFeatures = database.GetCollection<ProductFeature>("ProductFeatures");
        database.DropCollection("ProductFeatures"); 

        _repository = new ProductManagementRepo(_productFeatures);
    }

    private async Task SeedDataAsync()
    {
        await _productFeatures.InsertManyAsync(new[]
        {
            new ProductFeature { Id = Guid.NewGuid(), TargetCompletionDate = DateTime.UtcNow.AddDays(30), Status = Status.Active },
            new ProductFeature { Id = Guid.NewGuid(), TargetCompletionDate = DateTime.UtcNow.AddDays(-10), Status = Status.Closed }
        });
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCorrectFeature()
    {
        // Arrange
        await SeedDataAsync();
        var expectedFeature = (await _productFeatures.Find(_ => true).ToListAsync()).First();

        // Act
        var result = await _repository.GetByIdAsync(expectedFeature.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedFeature.Id, result.Id);
    }

    [Fact]
    public async Task CreateAsync_AddsFeatureSuccessfully()
    {
        // Arrange
        var newFeature = new ProductFeature { Id = Guid.NewGuid(), TargetCompletionDate = DateTime.UtcNow.AddDays(20), Status = Status.Active };

        // Act
        await _repository.CreateAsync(newFeature);
        var insertedFeature = await _productFeatures.Find(f => f.Id == newFeature.Id).FirstOrDefaultAsync();

        // Assert
        Assert.NotNull(insertedFeature);
        Assert.Equal(newFeature.Id, insertedFeature.Id);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesFeatureSuccessfully()
    {
        // Arrange
        await SeedDataAsync();
        var featureToUpdate = (await _productFeatures.Find(_ => true).ToListAsync()).First();
        featureToUpdate.Status = Status.Closed;

        // Act
        await _repository.UpdateAsync(featureToUpdate.Id, featureToUpdate);
        var updatedFeature = await _productFeatures.Find(f => f.Id == featureToUpdate.Id).FirstOrDefaultAsync();

        // Assert
        Assert.NotNull(updatedFeature);
        Assert.Equal(Status.Closed, updatedFeature.Status);
    }

    [Fact]
    public async Task DeleteAsync_DeletesFeatureSuccessfully()
    {
        // Arrange
        await SeedDataAsync();
        var featureToDelete = (await _productFeatures.Find(_ => true).ToListAsync()).First();

        // Act
        await _repository.DeleteAsync(featureToDelete.Id);
        var result = await _productFeatures.Find(f => f.Id == featureToDelete.Id).FirstOrDefaultAsync();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllFeatures()
    {
        // Arrange
        await SeedDataAsync();

        // Act
        var results = await _repository.GetAllAsync();

        // Assert
        Assert.Equal(2, results.Count());
    }
}