namespace AiMiniSample.Database_Tables;

public class User
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;

    public ICollection<Pet> Pets { get; set; } = new List<Pet>();
}
