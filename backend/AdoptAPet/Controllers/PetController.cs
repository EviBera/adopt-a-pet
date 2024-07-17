using System.ComponentModel.DataAnnotations;
using System.Data;
using AdoptAPet.DTOs.Pet;
using AdoptAPet.Mappers;
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
    public async Task<ActionResult<List<PetDto>>> GetAllAsync()
    {
        try
        {
            var pets = await _repository.GetAllAsync();
            return Ok(pets);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting pets.");
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{petId:int}")]
    public async Task<ActionResult<PetDto>> GetByIdAsync([Required, FromRoute]int petId)
    {
        try
        {
            var pet = await _repository.GetByIdAsync(petId);
            return Ok(pet);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting pet with id: " + petId);
            if (e is RowNotInTableException)
            {
                return NotFound($"The searched pet does not exist.");
            }
            return StatusCode(500, $"Error getting pet, {e.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<PetDto>> RegisterPetAsync([FromBody] CreatePetRequestDto petDto)
    {
        try
        {
            var newPet = await _repository.CreateAsync(petDto);
            return CreatedAtAction("GetbyId", new { petId = newPet.Id }, newPet.ToPetDto());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error registering the new pet.");
            return BadRequest($"Error registering the pet, {e.Message}");
        }
    }

    [HttpPatch("{petId:int}")]
    public async Task<ActionResult<PetDto>> UpdatePetAsync([Required, FromRoute] int petId,
        [FromBody] UpdatePetRequestDto petDto)
    {
        try
        {
            var pet = await _repository.UpdateAsync(petId, petDto);
            return Ok(pet.ToPetDto());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error updating pet with id " + petId);
            if (e is RowNotInTableException)
            {
                return BadRequest("Pet does not exist.");
            }

            return StatusCode(500, "Something went wrong.");
        }
    }

    [HttpDelete("{petId:int}")]
    public async Task<ActionResult> DeleteAsync([Required, FromRoute]int petId)
    {
        try
        {
            await _repository.DeleteAsync(petId);
            return NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting pet with id: " + petId);
            if (e is RowNotInTableException)
            {
                return BadRequest("Pet does not exist.");
            }

            return StatusCode(500, "Something went wrong.");
        }
    }
}