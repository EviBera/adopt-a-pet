using System.Data;
using AdoptAPet.Data;
using AdoptAPet.DTOs.Pet;
using AdoptAPet.Mappers;
using AdoptAPet.Models;
using Microsoft.EntityFrameworkCore;

namespace AdoptAPet.Services.Repositories;

public class PetRepository : IPetRepository
{

    private readonly AppDbContext _dbContext;

    public PetRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<IEnumerable<PetDto>> GetAllAsync()
    {
        var pets = await _dbContext.Pets.Select(p => p.ToPetDto()).ToListAsync();
        return pets;
    }

    public async Task<PetDto?> GetByIdAsync(int petId)
    {
        var pet = await _dbContext.Pets.FirstOrDefaultAsync(p => p.Id == petId);
        if (pet != null)
        {
            return pet.ToPetDto();
        }

        throw new RowNotInTableException();
    }

    public async Task<Pet> CreateAsync(CreatePetRequestDto requestDto)
    {
        var newPet = requestDto.ToPetFromCreatePetRequestDto();
        await _dbContext.Pets.AddAsync(newPet);
        await _dbContext.SaveChangesAsync();

        return newPet;
    }

    public async Task<Pet> UpdateAsync(int petId, UpdatePetRequestDto petDto)
    {
        var pet = await _dbContext.Pets.FirstOrDefaultAsync(p => p.Id == petId);
        
        if (pet == null)
        {
            throw new RowNotInTableException();
        }

        pet.Name = petDto.Name ?? pet.Name;
        pet.IsNeutered = petDto.IsNeutered ?? pet.IsNeutered;
        pet.Description = petDto.Description ?? pet.Description;
        pet.PictureLink = petDto.PictureLink ?? pet.PictureLink;

        await _dbContext.SaveChangesAsync();

        return pet;
    }

    public async Task DeleteAsync(int petId)
    {
        var pet = await _dbContext.Pets.FirstOrDefaultAsync(p => p.Id == petId);

        if (pet == null)
        {
            throw new RowNotInTableException();
        }

        _dbContext.Pets.Remove(pet);
        await _dbContext.SaveChangesAsync();
    }
}