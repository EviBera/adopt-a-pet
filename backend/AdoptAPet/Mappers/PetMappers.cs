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
        return new Pet
        {
            Name = petDto.Name,
            Species = petDto.Species,
            Birth = petDto.Birth,
            Gender = petDto.Gender,
            IsNeutered = petDto.IsNeutered,
            Description = petDto.Description,
            Owner = null,
            PictureLink = petDto.PictureLink
        };
    }
}