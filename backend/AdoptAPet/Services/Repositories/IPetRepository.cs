using AdoptAPet.Models;

namespace AdoptAPet.Services.Repositories;

public interface IPetRepository
{
    Task<IEnumerable<Pet>> GetAllAsync();
    Task<Pet?> GetByIdAsync(int petId);
    Task<Pet> CreateAsync(Pet pet);
    Task<Pet> UpdateAsync(Pet pet);
    Task DeleteAsync(Pet pet);
}