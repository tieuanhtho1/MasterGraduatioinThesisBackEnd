namespace WebAPI.Models.DTOs.MindMap
{
    public class CreateMindMapDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
