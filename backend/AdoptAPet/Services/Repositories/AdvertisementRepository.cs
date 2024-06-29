using System.Data;
using AdoptAPet.Data;
using AdoptAPet.DTOs.Advertisement;
using AdoptAPet.Mappers;
using AdoptAPet.Models;
using Microsoft.EntityFrameworkCore;

namespace AdoptAPet.Services.Repositories;

public class AdvertisementRepository : IAdvertisementRepository
{
    
    private readonly AppDbContext _dbContext;

    public AdvertisementRepository(AppDbContext context)
    {
        _dbContext = context;
    }
    public async Task<IEnumerable<AdvertisementDto>> GetAllAsync()
    {
        var ads = await _dbContext.Advertisements
            .Include(a => a.Pet)
            .Include(a => a.Applications)
            .Select(ad => ad.ToAdvertisementDto()).ToListAsync();
        return ads;
    }

    public async Task<IEnumerable<AdvertisementDto>> GetCurrentAdsAsync()
    {
        var ads = await _dbContext.Advertisements
            .Include(a => a.Pet)
            .Include(a => a.Applications)
            .Where(ad => ad.Pet.Owner == null)
            .Select(ad=> ad.ToAdvertisementDto())
            .ToListAsync();
        return ads;
    }

    public async Task<AdvertisementDto> GetByIdAsync(int advertisementId)
    {
        var ad = await _dbContext.Advertisements
            .Include(a => a.Pet)
            .Include(a => a.Applications)
            .FirstOrDefaultAsync(ad => ad.Id == advertisementId);
        if (ad != null)
        {
            return ad.ToAdvertisementDto();
        }

        throw new RowNotInTableException();
    }

    public async Task<Advertisement> CreateAsync(CreateAdvertisementRequestDto requestDto)
    {
        var pet = await _dbContext.Pets.FirstOrDefaultAsync(p => p.Id == requestDto.PetId);
        
        if (pet == null)
        {
            throw new RowNotInTableException();
        }
        
        var newAd = requestDto.ToAdvertisementFromCreateAdvertisementRequestDto();
        
        await _dbContext.Advertisements.AddAsync(newAd);
        await _dbContext.SaveChangesAsync();

        return newAd;
    }

    public async Task<Advertisement> UpdateAsync(int advertisementId, UpdateAdvertisementRequestDto requestDto)
    {
        var ad = await _dbContext.Advertisements
            .Include(ad => ad.Applications)
            .FirstOrDefaultAsync(ad => ad.Id == advertisementId);
        if (ad == null)
        {
            throw new RowNotInTableException();
        }

        ad.ExpiresAt = requestDto.ExpiresAt ?? ad.ExpiresAt;
        ad.Applications = requestDto.Applications ?? ad.Applications;

        await _dbContext.SaveChangesAsync();

        return ad;
    }

    public async Task DeleteAsync(int advertisementId)
    {
        var ad = await _dbContext.Advertisements.FirstOrDefaultAsync(ad => ad.Id == advertisementId);
        if (ad == null)
        {
            throw new RowNotInTableException();
        }

        _dbContext.Advertisements.Remove(ad);
        await _dbContext.SaveChangesAsync();
    }
}