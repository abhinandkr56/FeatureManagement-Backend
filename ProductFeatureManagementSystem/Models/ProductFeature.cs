using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProductFeatureManagementSystem.Models;

public class ProductFeature
{ 
    public Guid Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }

    //[Required]
    public Complexity EstimatedComplexity { get; set; }

   // [Required]
    public Status Status { get; set; } 

    public DateTime? TargetCompletionDate { get; set; }

    public DateTime? ActualCompletionDate { get; set; }
}