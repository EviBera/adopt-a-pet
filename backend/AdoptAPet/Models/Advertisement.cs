namespace AdoptAPet.Models;

public class Advertisement
{
    public int Id { get; set; }
    public int PetId { get; set; }
    public Pet? Pet { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime ExpiresAt { get; set; }
    public ICollection<Application> Applications { get; set; } = new List<Application>();
}   