using System.Data;
using AdoptAPet.DTOs.Application;
using AdoptAPet.Mappers;
using AdoptAPet.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AdoptAPet.Controllers;

[Route("api/application")]
[ApiController]
public class ApplicationController : ControllerBase
{
    private readonly ILogger<ApplicationController> _logger;
    private readonly IApplicationRepository _repository;

    public ApplicationController(ILogger<ApplicationController> logger, IApplicationRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<ApplicationDto>> GetByUserIdAsync([FromRoute]string userId)
    {
        try
        {
            var app = await _repository.GetByUserAsync(userId);
            return Ok(app);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting applications of user " + userId);
            if (e is RowNotInTableException)
            {
                return BadRequest("Invalid user id.");
            }

            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("app/{applicationId:int}")]
    public async Task<ActionResult<List<ApplicationDto>>> GetByIdAsync([FromRoute]int applicationId)
    {
        try
        {
            var apps = await _repository.GetByIdAsync(applicationId);
            return Ok(apps);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting application with id " + applicationId);
            if (e is RowNotInTableException)
            {
                return BadRequest("Invalid application id.");
            }

            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("ad/{advertisementId:int}")]
    public async Task<ActionResult<List<ApplicationDto>>> GetByAdvertisementId([FromRoute] int advertisementId)
    {
        try
        {
            var apps = await _repository.GetByAdvertisementAsync(advertisementId);
            return Ok(apps);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting applications of advertisement with id " + advertisementId);
            if (e is RowNotInTableException)
            {
                return BadRequest("Invalid advertisement id.");
            }

            return StatusCode(500, e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApplicationDto>> CreateAsync([FromBody] CreateApplicationRequestDto requestDto)
    {
        try
        {
            var newApplication = await _repository.CreateAsync(requestDto);
            return Ok(newApplication.ToApplicationDto());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to register the application.");
            if (e is RowNotInTableException)
            {
                return BadRequest("Invalid parameters.");
            }

            return StatusCode(500, e.Message);
        }
    }

    [HttpPost("{applicationId:int}")]
    public async Task<ActionResult<ApplicationDto>> UpdateAsync([FromRoute] int applicationId,
        [FromBody] UpdateApplicationRequestDto requestDto)
    {
        try
        {
            var app = await _repository.UpdateAsync(applicationId, requestDto);
            return Ok(app.ToApplicationDto());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to update the application.");
            if (e is RowNotInTableException)
            {
                return BadRequest("Invalid application id.");
            }

            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete("{applicationId:int}")]
    public async Task<ActionResult> DelateAsync([FromRoute] int applicationId)
    {
        try
        {
            await _repository.DeleteAsync(applicationId);
            return Ok("Application deleted.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to delete the application.");
            if (e is RowNotInTableException)
            {
                return BadRequest("Invalid application id.");
            }

            return StatusCode(500, e.Message);
        }
    }
}