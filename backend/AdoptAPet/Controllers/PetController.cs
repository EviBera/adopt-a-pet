using AdoptAPet.Models;
using AdoptAPet.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AdoptAPet.Controllers;

[Route("api/pet")]
[ApiController]
public class PetController : ControllerBase
{
    private readonly ILogger<PetController> _logger;
    private readonly IPetRepository _repository;

    public PetController(ILogger<PetController> logger, IPetRepository petRepository)
    {
        _logger = logger;
        _repository = petRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<Pet>>> GetAllAsync()
    {
        try
        {
            var pets = await _repository.GetAllAsync();
            return Ok(pets.ToList());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting pets.");
            return StatusCode(500, e.Message);
        }
    }
}