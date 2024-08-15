using AdoptAPet.DTOs.Pet;
using AdoptAPet.Models;

namespace AdoptAPet.Mappers;

public static class PetMappers
{
    public static PetDto ToPetDto(this Pet petModel)
    {
        return new PetDto
        {
            Id = petModel.Id,
            Name = petModel.Name,
            Species = Enum.GetName(typeof(Species), petModel.Species),
            Birth = petModel.Birth,
            Gender = Enum.GetName(typeof(Gender), petModel.Gender),
            IsNeutered = petModel.IsNeutered,
            Description = petModel.Description,
            OwnerId = petModel.Owner != null ? petModel.Owner.Id : string.Empty,
            PictureLink = petModel.PictureLink
        };
    }

    public static Pet ToPetFromCreatePetRequestDto(this CreatePetRequestDto petDto)
    {
        if (Enum.IsDefined(typeof(Species), (Species) Enum.Parse(typeof(Species), petDto.Species)) && Enum.IsDefined(typeof(Gender), (Gender) Enum.Parse(typeof(Gender), petDto.Gender)))
        {
            return new Pet
                    {
                        Name = petDto.Name,
                        Species = (Species) Enum.Parse(typeof(Species), petDto.Species),
                        Birth = DateTimeOffset.Parse(petDto.Birth).UtcDateTime,
                        Gender = (Gender) Enum.Parse(typeof(Gender), petDto.Gender),
                        IsNeutered = petDto.IsNeutered,
                        Description = petDto.Description,
                        Owner = null,
                        PictureLink = petDto.PictureLink
                    };
        }
        
        throw new ArgumentException("Invalid species or gender.");
    }
}