namespace WebAPI.Models
{
    public class FlashCard
    {
        public int Id { get; set; }

        public string Term { get; set; } = string.Empty;
        public string Definition { get; set; } = string.Empty;
        public int Score { get; set; }

        // Foreign key to the collection it belongs to
        public int FlashCardCollectionId { get; set; }
        public FlashCardCollection FlashCardCollection { get; set; } = null!;
    }
}