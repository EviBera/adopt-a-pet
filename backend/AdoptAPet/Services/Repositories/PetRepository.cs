using AdoptAPet.Models;

namespace AdoptAPet.Services.Repositories;

public class PetRepository : IPetRepository
{
    public Task<IEnumerable<Pet>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Pet?> GetByIdAsync(int petId)
    {
        throw new NotImplementedException();
    }

    public Task<Pet> CreateAsync(Pet pet)
    {
        throw new NotImplementedException();
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