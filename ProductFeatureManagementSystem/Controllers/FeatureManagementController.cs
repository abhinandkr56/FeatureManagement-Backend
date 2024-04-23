using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ProductFeatureManagementSystem.Models;
using ProductFeatureManagementSystem.Services;

namespace ProductFeatureManagementSystem.Controllers;

[ApiController]
[Route("api/productfeatures")]
[EnableCors("AllowSpecificOrigin")]
public class FeatureManagementController : ControllerBase
{
    private readonly IProductManagementService _service;

    public FeatureManagementController(IProductManagementService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductFeature>>> GetAllFeatures()
    {
        try
        {
            var features = await _service.GetAllFeaturesAsync();
            return Ok(features);
        }
        catch (System.Exception)
        {
            return StatusCode(500, "An error occurred while retrieving data.");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFeature(Guid id)
    {
        try
        {
            var feature = await _service.GetFeatureAsync(id);
            if (feature == null)
                return NotFound("Feature not found.");

            return Ok(feature);
        }
        catch (System.Exception)
        {
            return StatusCode(500, "An error occurred while retrieving the feature.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateFeature([FromBody] ProductFeature feature)
    {
        try
        {
            await _service.AddFeatureAsync(feature);
            return CreatedAtAction(nameof(GetFeature), new { id = feature.Id }, feature);
        }
        catch (System.Exception ex)
        {
            return StatusCode(500, "An error occurred while creating the feature.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFeature(Guid id, [FromBody] ProductFeature feature)
    {
        try
        {
            if (await _service.GetFeatureAsync(id) == null)
                return NotFound("Feature not found.");

            await _service.UpdateFeatureAsync(id, feature);
            return NoContent();
        }
        catch (System.Exception)
        {
            return StatusCode(500, "An error occurred while updating the feature.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFeature(Guid id)
    {
        try
        {
            if (await _service.GetFeatureAsync(id) == null)
                return NotFound("Feature not found.");

            await _service.DeleteFeatureAsync(id);
            return NoContent();
        }
        catch (System.Exception)
        {
            return StatusCode(500, "An error occurred while deleting the feature.");
        }
    }
}