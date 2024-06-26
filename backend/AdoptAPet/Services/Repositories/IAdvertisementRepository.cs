using AdoptAPet.DTOs.Advertisement;
using AdoptAPet.DTOs.Pet;
using AdoptAPet.Models;

namespace AdoptAPet.Services.Repositories;

public interface IAdvertisementRepository
{
    Task<IEnumerable<AdvertisementDto>> GetAllAsync();
    Task<IEnumerable<AdvertisementDto>> GetCurrentAdsAsync();
    Task<AdvertisementDto> GetByIdAsync(int advertisementId);
    Task<Advertisement> CreateAsync(CreateAdvertisementRequestDto requestDto);
    Task<Advertisement> UpdateAsync(int advertisementId, UpdateAdvertisementRequestDto requestDto);
    Task DeleteAsync(int advertisementId);
}