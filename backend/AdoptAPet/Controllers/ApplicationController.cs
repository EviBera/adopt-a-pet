using System.Data;
using System.Security.Claims;
using AdoptAPet.DTOs.Application;
using AdoptAPet.Mappers;
using AdoptAPet.Services.Repositories;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Rescue Team, Admin")]
    public async Task<ActionResult<List<ApplicationDto>>> GetByUserIdAsync([FromRoute]string userId)
    {
        try
        {
            var apps = await _repository.GetByUserAsync(userId);
            return Ok(apps);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting applications of user.");
            return e is RowNotInTableException ? BadRequest("Invalid user id.") : StatusCode(500, e.Message);
        }
    }
    [HttpGet("mine")]
    [Authorize(Roles = "User")]
    public async Task<ActionResult<List<ApplicationDto>>> GetByUserIdAsync()
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return BadRequest(new { message = "Logged-in user is missing." });
            }
            
            var apps = await _repository.GetByUserAsync(userId);
            return Ok(apps);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting applications of user.");
            return e is RowNotInTableException ? BadRequest("Invalid user id.") : StatusCode(500, e.Message);
        }
    }

    [HttpGet("app/{applicationId:int}")]
    [Authorize(Roles = "User, Rescue Team, Admin")]
    public async Task<ActionResult<ApplicationDto>> GetByIdAsync([FromRoute]int applicationId)
    {
        try
        {
            var app = await _repository.GetByIdAsync(applicationId);
            return Ok(app);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting application with id " + applicationId);
            return e is RowNotInTableException ? BadRequest("Invalid application id.") : StatusCode(500, e.Message);
        }
    }

    [HttpGet("ad/{advertisementId:int}")]
    [Authorize(Roles = "Rescue Team, Admin")]
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
            return e is RowNotInTableException ? BadRequest("Invalid advertisement id.") : StatusCode(500, e.Message);
        }
    }

    [HttpPost]
    [Authorize(Roles = "Rescue Team, Admin")]
    public async Task<ActionResult<ApplicationDto>> CreateAsync([FromBody] CreateApplicationRequestDto requestDto)
    {
        try
        {
            var newApplication = await _repository.CreateAsync(requestDto);
            return CreatedAtAction("GetById", new { applicationId = newApplication.Id}, newApplication.ToApplicationDto());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to register the application.");
            return e is RowNotInTableException ? BadRequest("Invalid parameters.") : StatusCode(500, e.Message);
        }
    }

    [HttpPost("mine")]
    [Authorize(Roles = "User")]
    public async Task<ActionResult<ApplicationDto>> CreateAsync([FromBody] int advertisementId)
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return BadRequest(new { message = "Logged-in user is missing." });
            }

            CreateApplicationRequestDto requestDto = new CreateApplicationRequestDto
            {
                AdvertisementId = advertisementId,
                UserId = userId
            };
            var newApplication = await _repository.CreateAsync(requestDto);
            return CreatedAtAction("GetById", new { applicationId = newApplication.Id}, newApplication.ToApplicationDto());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to register the application.");
            return e is RowNotInTableException ? BadRequest("Invalid parameters.") : StatusCode(500, e.Message);
        }
    }

    [HttpPatch("{applicationId:int}")]
    [Authorize(Roles = "Rescue Team, Admin")]
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
            return e is RowNotInTableException ? BadRequest("Invalid application id.") : StatusCode(500, e.Message);
        }
    }

    [HttpDelete("{applicationId:int}")]
    [Authorize(Roles = "User, Rescue Team, Admin")]
    public async Task<ActionResult> DeleteAsync([FromRoute] int applicationId)
    {
        try
        {
            await _repository.DeleteAsync(applicationId);
            return NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to delete the application.");
            return e is RowNotInTableException ? BadRequest("Invalid application id.") : StatusCode(500, e.Message);
        }
    }
}