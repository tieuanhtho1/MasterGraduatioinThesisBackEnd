using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.DTOs.AIGeneration;

public class GenerateMindMapRequest
{
    [Required]
    public int CollectionId { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    [Required]
    public string Provider { get; set; }
    
    [Required]
    public string Model { get; set; }
}
