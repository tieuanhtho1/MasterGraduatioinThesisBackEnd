namespace WebAPI.Models.DTOs.UserApiKey;

public class UserApiKeyResponse
{
    public int Id { get; set; }
    public string Provider { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
