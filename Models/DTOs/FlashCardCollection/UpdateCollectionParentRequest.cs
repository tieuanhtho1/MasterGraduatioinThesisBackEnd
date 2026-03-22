namespace WebAPI.Models.DTOs.FlashCardCollection;

public class UpdateCollectionParentRequest
{
    /// <summary>
    /// The new parent collection ID. Set to null to make it a root collection.
    /// </summary>
    public int? ParentId { get; set; }
}
