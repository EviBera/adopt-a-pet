namespace AdoptAPet.Models;

public class Advertisement
{
    public int Id { get; init; }
    public int PetId { get; init; }
    public Pet Pet { get; init; } = null!;
    public DateTime CreatedAt { get; init; } = DateTime.Now;
    public DateTime ExpiresAt { get; set; }
    public ICollection<Application> Applications { get; set; } = new List<Application>();
}   