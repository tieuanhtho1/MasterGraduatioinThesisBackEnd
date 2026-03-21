using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.DTOs.UserApiKey;

public class UpdateUserApiKeyRequest
{
    [Required]
    public string Provider { get; set; } = string.Empty;

    [Required]
    public string ApiKey { get; set; } = string.Empty;
}
