namespace AdoptAPet.Models;

public class Advertisement
{
    public int Id { get; set; }
    public Pet Pet { get; set; }
    public List<Application> Applications { get; set; }
}   