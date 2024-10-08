using System.ComponentModel.DataAnnotations;
using System.Data;
using AdoptAPet.DTOs.Advertisement;
using AdoptAPet.Mappers;
using AdoptAPet.Services.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdoptAPet.Controllers;

[Route("api/advertisement")]
[ApiController]
public class AdvertisementController : ControllerBase
{
    private readonly ILogger<AdvertisementController> _logger;
    private readonly IAdvertisementRepository _repository;

    public AdvertisementController(ILogger<AdvertisementController> logger, IAdvertisementRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet]
    [Authorize(Roles = "Rescue Team, Admin")]
    public async Task<ActionResult<List<AdvertisementDto>>> GetAllAsync()
    {
        try
        {
            var ads = await _repository.GetAllAsync();
            return Ok(ads);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting advertisements.");
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{advertisementId:int}")]
    public async Task<ActionResult<AdvertisementDto>> GetByIdAsync([Required, FromRoute]int advertisementId)
    {
        try
        {
            var ad = await _repository.GetByIdAsync(advertisementId);
            return Ok(ad);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting advertisement with id " + advertisementId);
            return e is RowNotInTableException ? NotFound("The searched advertisement does not exist.") : StatusCode(500, e.Message);
        }
    }

    [HttpGet("current")]
    public async Task<ActionResult<AdvertisementDto>> GetCurrentAdsAsync()
    {
        try
        {
            var ads = await _repository.GetCurrentAdsAsync();
            return Ok(ads);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting current advertisements.");
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost]
    [Authorize(Roles = "Rescue Team, Admin")]
    public async Task<ActionResult<AdvertisementDto>> CreateAdvertisement(
        [Required, FromBody] CreateAdvertisementRequestDto requestDto)
    {
        try
        {
            var newAd = await _repository.CreateAsync(requestDto);
            return CreatedAtAction("GetById", new { advertisementId = newAd.Id }, newAd.ToAdvertisementDto());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error creating new advertisement.");
            if (e is RowNotInTableException)
            {
                return BadRequest("The pet does not exist.");
            }
            
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete("{advertisementId:int}")]
    [Authorize(Roles = "Rescue Team, Admin")]
    public async Task<ActionResult> DeleteAsync([FromRoute] int advertisementId)
    {
        try
        {
            await _repository.DeleteAsync(advertisementId);
            return NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to delete the advertisement.");
            return e is RowNotInTableException ? BadRequest("Invalid advertisement id.") : StatusCode(500, e.Message);
        }
    }
}