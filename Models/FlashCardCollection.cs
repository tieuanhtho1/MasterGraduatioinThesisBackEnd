namespace WebAPI.Models
{
    public class FlashCardCollection
    {
        public int Id { get; set; }

        // Belongs to a user
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        // Self-referencing parent
        public int? ParentId { get; set; }
        public FlashCardCollection? Parent { get; set; }
        public List<FlashCardCollection> Children { get; set; } = new();

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // One collection → many flashcards
        public List<FlashCard> FlashCards { get; set; } = new();
    }
}