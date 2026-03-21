namespace WebAPI.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.User;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }

    // One user → Many collections
    public List<FlashCardCollection> FlashCardCollections { get; set; } = new();

    // One user → Many mind maps
    public List<MindMap> MindMaps { get; set; } = new();

    // One user → Many API keys
    public List<UserApiKey> ApiKeys { get; set; } = new();
}
