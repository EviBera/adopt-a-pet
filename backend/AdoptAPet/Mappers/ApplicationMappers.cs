using AdoptAPet.DTOs.Application;
using AdoptAPet.Models;

namespace AdoptAPet.Mappers;

public static class ApplicationMappers
{
    public static ApplicationDto ToApplicationDto(this Application applicationModel)
    {
        return new ApplicationDto
        {
            Id = applicationModel.Id,
            UserId = applicationModel.UserId,
            AdvertisementId = applicationModel.AdvertisementId,
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