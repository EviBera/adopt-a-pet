using System.ComponentModel.DataAnnotations;
namespace AdoptAPet.Models;

public class Pet
{
    [Key]
    public int Id { get; init; }
    [MaxLength(30)]
    public string Name { get; set; } = String.Empty;
    public Species Species { get; init; }
    public DateTime Birth { get; set; } 
    public Gender Gender { get; init; }
    public bool IsNeutered { get; set; }
    [MaxLength(300)] public string Description { get; set; } = String.Empty;
    public User? Owner { get; set; }
    [MaxLength(100)]
    public string PictureLink { get; set; } = String.Empty;

    public virtual ICollection<Advertisement> Advertisements { get; set; } = new List<Advertisement>();
}