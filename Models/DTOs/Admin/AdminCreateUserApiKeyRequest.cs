using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.DTOs.Admin;

public class AdminCreateUserApiKeyRequest
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public string Provider { get; set; } = string.Empty;

    [Required]
    public string ApiKey { get; set; } = string.Empty;
}
