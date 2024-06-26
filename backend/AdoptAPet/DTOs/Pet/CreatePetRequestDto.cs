using System.ComponentModel.DataAnnotations;
using AdoptAPet.Models;

namespace AdoptAPet.DTOs.Pet;

public class CreatePetRequestDto
{
    [MaxLength(30)]
    public string Name { get; set; } = String.Empty;
    public Species Species { get; init; }
    public DateTime Birth { get; set; } 
    public Gender Gender { get; init; }
    public bool IsNeutered { get; set; }
    [MaxLength(300)] public string Description { get; set; } = String.Empty;
    [MaxLength(100)]
    public string PictureLink { get; set; } = String.Empty;
}