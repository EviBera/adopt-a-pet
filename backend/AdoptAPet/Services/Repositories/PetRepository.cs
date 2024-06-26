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

    public async Task<Pet> CreateAsync(CreatePetRequestDto newPet)
    {
        var pet = newPet.ToPetFromCreatePetRequestDto();
        await _dbContext.Pets.AddAsync(pet);
        await _dbContext.SaveChangesAsync();

        return pet;
    }

    public Task<Pet> UpdateAsync(Pet pet)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Pet pet)
    {
        throw new NotImplementedException();
    }
}