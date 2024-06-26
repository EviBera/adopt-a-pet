using AdoptAPet.DTOs.Pet;
using AdoptAPet.Models;

namespace AdoptAPet.Services.Repositories;

public interface IPetRepository
{
    Task<IEnumerable<PetDto>> GetAllAsync();
    Task<PetDto?> GetByIdAsync(int petId);
    Task<Pet> CreateAsync(CreatePetRequestDto newPet);
    Task<Pet> UpdateAsync(Pet pet);
    Task DeleteAsync(Pet pet);
}