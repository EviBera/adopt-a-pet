using System.ComponentModel.DataAnnotations;

namespace AdoptAPet.Models;

public class Application
{
    [Key]
    public int Id { get; set; }
    [MaxLength(100)]
    public string UserId { get; set; } = string.Empty;
    public User? User { get; set; }
    public int AdvertisementId { get; set; }
    public Advertisement Advertisement { get; set; } = null!;
    public bool? IsAccepted { get; set; } = null;
}