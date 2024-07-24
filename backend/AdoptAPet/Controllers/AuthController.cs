using System.Data;
using AdoptAPet.DTOs.User;
using AdoptAPet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace AdoptAPet.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<AuthController> _logger;

    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AuthController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<User?>> GetByIdAsync([FromRoute] string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user == null ? NotFound() : Ok(user);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting user.");
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterAsync([FromBody] RegisterUserRequestDto requestDto)
    {
        try
        {
            var user = new User
            {
                UserName = requestDto.Email,
                Email = requestDto.Email,
                FirstName = requestDto.FirstName,
                LastName = requestDto.LastName
            };

            var newUser = await _userManager.CreateAsync(user, requestDto.Password);

            if (newUser.Succeeded)
            {
                return CreatedAtAction("GetById", new {userId = user.Id}, new NewUserDto
                {
                    Id = user.Id,
                    UserName = user.Email,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Token = string.Empty

                });
            }
            else
            {
                return StatusCode(500, newUser.Errors);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to register new user.");
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete("{userId}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] string userId)
    {
        try
        {
            User? user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("The user does not exist");
            }
            
            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded ? NoContent() : StatusCode(500, "Something went wrong, please try again later.");
            
        }
        catch (Exception e)
        {
            _logger.LogError("Error deleting user.");
            return StatusCode(500, e.Message);
        }
    }
}