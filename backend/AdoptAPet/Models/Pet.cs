using System.ComponentModel.DataAnnotations;
namespace AdoptAPet.Models;

public class Pet
{
    [Key]
    public int Id { get; init; }
    public string Name { get; set; } = String.Empty;
    public Species Species { get; init; }
    public DateTime Birth { get; set; } 
    public Gender Gender { get; init; }
    public bool IsNeutered { get; set; }
    public string Description { get; set; }
    public User? Owner { get; set; }
    public string PictureLink { get; set; }
}