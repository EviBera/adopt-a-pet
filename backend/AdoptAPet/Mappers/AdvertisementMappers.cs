using AdoptAPet.DTOs.Advertisement;
using AdoptAPet.Models;

namespace AdoptAPet.Mappers;

public static class AdvertisementMappers
{
    public static AdvertisementDto ToAdvertisementDto(this Advertisement advertisementModel)
    {
        return new AdvertisementDto
        {
            Id = advertisementModel.Id,
            PetDto = advertisementModel.Pet.ToPetDto(),
            CreatedAt = advertisementModel.CreatedAt,
            ExpiresAt = advertisementModel.ExpiresAt,
            Applications = advertisementModel.Applications.Select(app => app.ToApplicationDto()).ToList()
        };
    }

    public static Advertisement ToAdvertisementFromCreateAdvertisementRequestDto(
        this CreateAdvertisementRequestDto requestDto)
    {
        return new Advertisement
        {
            PetId = requestDto.PetId,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.SpecifyKind(requestDto.ExpiresAt, DateTimeKind.Utc),
            Applications = new List<Application>()
        };
    }
}