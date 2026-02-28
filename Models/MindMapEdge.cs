namespace WebAPI.Models
{
    public class MindMapEdge
    {
        public int Id { get; set; }

        // Source node reference
        public int SourceNodeId { get; set; }
        public MindMapNode SourceNode { get; set; } = null!;

        // Target node reference
        public int TargetNodeId { get; set; }
        public MindMapNode TargetNode { get; set; } = null!;

        // Handle identifiers (e.g., "bottom", "right", "top", "left")
        public string SourceHandle { get; set; } = string.Empty;
        public string TargetHandle { get; set; } = string.Empty;

        // Belongs to a mind map
        public int MindMapId { get; set; }
        public MindMap MindMap { get; set; } = null!;
    }
}
