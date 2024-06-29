using AdoptAPet.Models;

namespace AdoptAPet.DTOs.Pet;

public class PetDto
{
    public int Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public string? Species { get; init; }
    public DateTime Birth { get; set; } 
    public string? Gender { get; init; }
    public bool IsNeutered { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? OwnerId { get; set; } = string.Empty;
    public string PictureLink { get; set; } = string.Empty;
}