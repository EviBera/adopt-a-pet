using System.ComponentModel.DataAnnotations;
using AdoptAPet.Models;

namespace AdoptAPet.DTOs.Pet;

public class UpdatePetRequestDto
{
    [MaxLength(30)]
    public string? Name { get; set; }
    public bool? IsNeutered { get; set; }
    [MaxLength(500)] public string? Description { get; set; }
    [MaxLength(200)] public string? PictureLink { get; set; }
}