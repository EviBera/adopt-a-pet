namespace AdoptAPet.DTOs.Application;

public class ApplicationDto
{
    public int Id { get; init; }
    public string UserId { get; init; } = string.Empty;
    public int AdvertisementId { get; init; }
    public DateTime AdvertisementExpiresAt { get; init; }
    public string PetName { get; init; } = string.Empty;
    public string? PetSpecies { get; init; } = string.Empty;
    public DateTime PetBirth { get; init; }
    public string? PetGender { get; init; } = string.Empty;
    public bool PetIsNeutered { get; init; }
    public string PetDescription { get; init; } = string.Empty;
    public string PetPictureLink { get; init; } = string.Empty;
    public bool? IsAccepted { get; init; } = null;
}