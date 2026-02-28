namespace WebAPI.Models
{
    public class MindMapNode
    {
        public int Id { get; set; }

        // Position for React Flow
        public double PositionX { get; set; }
        public double PositionY { get; set; }

        // Visual properties
        public string Color { get; set; } = "#ffffff";
        public bool HideChildren { get; set; } = false;

        // Belongs to a mind map
        public int MindMapId { get; set; }
        public MindMap MindMap { get; set; } = null!;

        // Reference to a flash card
        public int FlashCardId { get; set; }
        public FlashCard FlashCard { get; set; } = null!;

        // Edges where this node is the source or target
        public List<MindMapEdge> SourceEdges { get; set; } = new();
        public List<MindMapEdge> TargetEdges { get; set; } = new();
    }
}
