using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.DTOs.Admin;

public class AdminUpdateUserApiKeyRequest
{
    [Required]
    public string Provider { get; set; } = string.Empty;

    [Required]
    public string ApiKey { get; set; } = string.Empty;
}
