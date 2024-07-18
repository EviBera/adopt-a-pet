using System.Data;
using AdoptAPet.Data;
using AdoptAPet.DTOs.Application;
using AdoptAPet.Mappers;
using AdoptAPet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AdoptAPet.Services.Repositories;

public class ApplicationRepository : IApplicationRepository
{
    private readonly AppDbContext _dbContext;
    private readonly UserManager<User> _userManager;

    public ApplicationRepository(AppDbContext context, UserManager<User> userManager)
    {
        _dbContext = context;
        _userManager = userManager;
    }
    
    public async Task<ApplicationDto> GetByIdAsync(int applicationId)
    {
        var app = await _dbContext.Applications
            .Include(a => a.User)
            .Include(a => a.Advertisement)
            .ThenInclude(ad => ad.Pet)
            .FirstOrDefaultAsync(a => a.Id == applicationId);
        if (app == null)
        {
            throw new RowNotInTableException();
        }
        
        return app.ToApplicationDto();
    }

    public async Task<ICollection<ApplicationDto>> GetByUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new RowNotInTableException();
        }
        
        var apps = await _dbContext.Applications
            .Include(a => a.User)
            .Include(a => a.Advertisement)
            .ThenInclude(ad => ad.Pet)
            .Where(a => a.UserId == userId)
            .Select(a => a.ToApplicationDto())
            .ToListAsync();

        return apps;
    }

    public async Task<ICollection<ApplicationDto>> GetByAdvertisementAsync(int advertisementId)
    {
        var ad = await _dbContext.Advertisements.FirstOrDefaultAsync(ad => ad.Id == advertisementId);
        if (ad == null)
        {
            throw new RowNotInTableException();
        }

        var apps = await _dbContext.Applications
            .Include(a => a.User)
            .Include(a => a.Advertisement)
            .ThenInclude(advertisement => advertisement.Pet)
            .Where(app => app.AdvertisementId == advertisementId)
            .Select(a => a.ToApplicationDto())
            .ToListAsync();

        return apps;
    }

    public async Task<Application> CreateAsync(CreateApplicationRequestDto requestDto)
    {
        var user = await _userManager.FindByIdAsync(requestDto.UserId);
        var ad = await _dbContext.Advertisements
            .Include(a => a.Pet)
            .FirstOrDefaultAsync(a => a.Id == requestDto.AdvertisementId);
        if (user == null || ad == null)
        {
            throw new RowNotInTableException();
        }
        
        var newApp = requestDto.ToApplicationFromCreateApplicationRequestDto();
        newApp.User = user;
        newApp.Advertisement = ad;
        
        await _dbContext.Applications.AddAsync(newApp);
        await _dbContext.SaveChangesAsync();
        
        await _dbContext.Entry(newApp).Reference(a => a.Advertisement).LoadAsync();
        await _dbContext.Entry(newApp.Advertisement).Reference(ad => ad.Pet).LoadAsync();
        
        return newApp;
    }
    
    public async Task<Application> UpdateAsync(int applicationId, UpdateApplicationRequestDto requestDto)
    {
        var app = await _dbContext.Applications
            .Include(a => a.Advertisement)
            .ThenInclude(advertisement => advertisement.Pet)
            .FirstOrDefaultAsync(a => a.Id == applicationId);
        if (app == null)
        {
            throw new RowNotInTableException();
        }

        app.IsAccepted = requestDto.IsAccepted;

        await _dbContext.SaveChangesAsync();

        return app;
    }

    public async Task DeleteAsync(int applicationId)
    {
        var app = await _dbContext.Applications.FirstOrDefaultAsync(a => a.Id == applicationId);
        if (app == null)
        {
            throw new RowNotInTableException();
        }
        
        _dbContext.Applications.Remove(app);
        await _dbContext.SaveChangesAsync();
    }
}