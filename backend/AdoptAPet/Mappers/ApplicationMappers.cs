using AdoptAPet.DTOs.Application;
using AdoptAPet.Models;

namespace AdoptAPet.Mappers;

public static class ApplicationMappers
{
    public static ApplicationDto ToApplicationDto(this Application applicationModel)
    {
        if (applicationModel == null) throw new ArgumentNullException(nameof(applicationModel));
        if (applicationModel.Advertisement == null) throw new ArgumentNullException(nameof(applicationModel.Advertisement));
        if (applicationModel.Advertisement.Pet == null) throw new ArgumentNullException(nameof(applicationModel.Advertisement.Pet));
        
        return new ApplicationDto
        {
            Id = applicationModel.Id,
            UserId = applicationModel.UserId,
            AdvertisementId = applicationModel.AdvertisementId,
            AdvertisementExpiresAt = applicationModel.Advertisement.ExpiresAt,
            PetName = applicationModel.Advertisement.Pet.Name,
            PetBirth = applicationModel.Advertisement.Pet.Birth,
            PetGender = Enum.GetName(typeof(Gender), applicationModel.Advertisement.Pet.Gender),
            PetSpecies = Enum.GetName(typeof(Species), applicationModel.Advertisement.Pet.Species),
            PetIsNeutered = applicationModel.Advertisement.Pet.IsNeutered,
            PetDescription = applicationModel.Advertisement.Pet.Description,
            PetPictureLink = applicationModel.Advertisement.Pet.PictureLink,
            IsAccepted = applicationModel.IsAccepted
        };
    }

    public static Application ToApplicationFromCreateApplicationRequestDto(this CreateApplicationRequestDto requestDto)
    {
        return new Application
        {
            UserId = requestDto.UserId,
            AdvertisementId = requestDto.AdvertisementId,
            IsAccepted = null
        };
    }
}