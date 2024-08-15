using System.ComponentModel.DataAnnotations;
using AdoptAPet.Models;

namespace AdoptAPet.DTOs.Pet;

public class CreatePetRequestDto
{
    [MaxLength(30)]
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required] 
    public string Species { get; init; } = string.Empty;

    public string Birth { get; set; } = string.Empty;
    public string Gender { get; init; } = string.Empty;
    public bool IsNeutered { get; set; }
    [MaxLength(500)] public string Description { get; set; } = String.Empty;
    [MaxLength(2000)]
    [Required]
    public string PictureLink { get; set; } = String.Empty;
}