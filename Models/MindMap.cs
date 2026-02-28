namespace WebAPI.Models
{
    public class MindMap
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Belongs to a user
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        // Belongs to a collection
        public int FlashCardCollectionId { get; set; }
        public FlashCardCollection FlashCardCollection { get; set; } = null!;

        // One mind map → many nodes
        public List<MindMapNode> Nodes { get; set; } = new();

        // One mind map → many edges
        public List<MindMapEdge> Edges { get; set; } = new();
    }
}
