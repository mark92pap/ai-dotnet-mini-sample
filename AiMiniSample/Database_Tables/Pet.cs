namespace AiMiniSample.Database_Tables;

public class Pet
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public int ApiId { get; set; } = default!;
    public string UserId { get; set; } = default!;
    
    // Navigation property
    public User User { get; set; } = default!;
}
