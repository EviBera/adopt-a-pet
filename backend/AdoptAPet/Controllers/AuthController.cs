using AdoptAPet.DTOs.User;
using AdoptAPet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
                return Ok(new NewUserDto
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
}