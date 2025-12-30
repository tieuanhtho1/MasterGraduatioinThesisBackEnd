using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class MindMapNode
    {
        [Key]
        public int Id { get; set; }

        // Foreign key to MindMap
        [Required]
        public int MindMapId { get; set; }

        [ForeignKey("MindMapId")]
        public MindMap MindMap { get; set; } = null!;

        // Foreign key to FlashCard
        [Required]
        public int FlashCardId { get; set; }

        [ForeignKey("FlashCardId")]
        public FlashCard FlashCard { get; set; } = null!;

        // Self-referencing for parent node (null for root nodes)
        public int? ParentNodeId { get; set; }

        [ForeignKey("ParentNodeId")]
        public MindMapNode? ParentNode { get; set; }

        // Navigation property for child nodes
        public ICollection<MindMapNode> ChildNodes { get; set; } = new List<MindMapNode>();

        // Position on the canvas
        [Required]
        public double PositionX { get; set; }

        [Required]
        public double PositionY { get; set; }

        // Visual properties
        [MaxLength(50)]
        public string Color { get; set; } = "#3B82F6"; // Default blue color

        public bool HideChildren { get; set; } = false;

        // Metadata
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}
