using AdoptAPet.DTOs.Application;
using AdoptAPet.DTOs.Pet;
using AdoptAPet.Models;

namespace AdoptAPet.DTOs.Advertisement;

public class AdvertisementDto
{
    public int Id { get; init; }
    public PetDto PetDto { get; init; } = null!;
    public DateTime CreatedAt { get; init; } 
    public DateTime ExpiresAt { get; set; }
    public ICollection<ApplicationDto>? Applications { get; set; }
}