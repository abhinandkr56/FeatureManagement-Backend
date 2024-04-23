using Moq;
using ProductFeatureManagementSystem.Exceptions;
using ProductFeatureManagementSystem.Models;
using ProductFeatureManagementSystem.Repositories;
using ProductFeatureManagementSystem.Services;
using Xunit;

namespace ProductFeatureManagementSystem.Tests;

public class ProductManagementServiceTests
{
    private readonly Mock<IProductManagementRepo> _mockRepo;
    private readonly ProductManagementService _service;

    public ProductManagementServiceTests()
    {
        _mockRepo = new Mock<IProductManagementRepo>();
        _service = new ProductManagementService(_mockRepo.Object);
    }
    
    [Fact]
    public async Task GetFeatureAsync_ReturnsCorrectFeature()
    {
        // Arrange
        var featureId = Guid.NewGuid();
        var expectedFeature = new ProductFeature { Id = featureId };
        _mockRepo.Setup(repo => repo.GetByIdAsync(featureId)).ReturnsAsync(expectedFeature);

        // Act
        var result = await _service.GetFeatureAsync(featureId);

        // Assert
        Assert.Equal(expectedFeature, result);
    }
    
    [Fact]
    public async Task AddFeatureAsync_ValidatesFutureDate_ThrowsException()
    {
        // Arrange
        var feature = new ProductFeature 
        { 
            Status = Status.Active, 
            TargetCompletionDate = DateTime.Today.AddDays(-1) 
        };

        // Act & Assert
        await Assert.ThrowsAsync<FutureDateRequiredException>(() => _service.AddFeatureAsync(feature));
    }

    [Fact]
    public async Task AddFeatureAsync_ValidFeature_AddsSuccessfully()
    {
        // Arrange
        var feature = new ProductFeature 
        { 
            Status = Status.Active, 
            TargetCompletionDate = DateTime.Today.AddDays(10) 
        };
        _mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<ProductFeature>())).Returns(Task.CompletedTask);

        // Act
        await _service.AddFeatureAsync(feature);

        // Assert
        _mockRepo.Verify(repo => repo.CreateAsync(It.IsAny<ProductFeature>()), Times.Once);
    }
    
    [Fact]
    public async Task UpdateFeatureAsync_CallsUpdateOnRepo()
    {
        // Arrange
        var featureId = Guid.NewGuid();
        var feature = new ProductFeature { Id = featureId };
        _mockRepo.Setup(repo => repo.UpdateAsync(featureId, feature)).Returns(Task.CompletedTask);

        // Act
        await _service.UpdateFeatureAsync(featureId, feature);

        // Assert
        _mockRepo.Verify(repo => repo.UpdateAsync(featureId, feature), Times.Once);
    }
    
    [Fact]
    public async Task DeleteFeatureAsync_CallsDeleteOnRepo()
    {
        // Arrange
        var featureId = Guid.NewGuid();
        _mockRepo.Setup(repo => repo.DeleteAsync(featureId)).Returns(Task.CompletedTask);

        // Act
        await _service.DeleteFeatureAsync(featureId);

        // Assert
        _mockRepo.Verify(repo => repo.DeleteAsync(featureId), Times.Once);
    }
    
    [Fact]
    public async Task GetAllFeaturesAsync_ReturnsAllFeatures()
    {
        // Arrange
        var expectedFeatures = new List<ProductFeature> { new ProductFeature(), new ProductFeature() };
        _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedFeatures);

        // Act
        var result = await _service.GetAllFeaturesAsync();

        // Assert
        Assert.Equal(expectedFeatures, result);
    }
}