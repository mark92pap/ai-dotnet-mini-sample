namespace AiMiniSample.Apis.Models;

/// <summary>
/// DTO representing a Pet from the external PetStore API
/// This is a local model used when the PetStoreClient is not available
/// </summary>
public class PetStoreDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Status { get; set; }
}
