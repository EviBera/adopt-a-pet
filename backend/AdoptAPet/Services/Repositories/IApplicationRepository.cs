using AdoptAPet.DTOs.Application;
using AdoptAPet.Models;

namespace AdoptAPet.Services.Repositories;

public interface IApplicationRepository
{
    Task<ApplicationDto> GetByIdAsync(int applicationId);
    Task<ICollection<ApplicationDto>> GetByUserAsync(string userId);
    Task<ICollection<ApplicationDto>> GetByAdvertisementAsync(int advertisementId);
    Task<Application> CreateAsync(CreateApplicationRequestDto requestDto);
    Task<Application> UpdateAsync(int applicationId, UpdateApplicationRequestDto requestDto);
    Task DeleteAsync(int applicationId);
}