using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.DTOs.AIGeneration;

public class GenerateAllRequest
{
    [Required]
    public IFormFile File { get; set; }
    
    [Required]
    public int ParentCollectionId { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    [Required]
    public string Provider { get; set; }
    
    [Required]
    public string Model { get; set; }
}
