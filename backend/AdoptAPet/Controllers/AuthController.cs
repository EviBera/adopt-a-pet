using AdoptAPet.DTOs.User;
using AdoptAPet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    [Authorize(Roles = "Rescue Team, Admin")]
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
        if (!ModelState.IsValid)
        {
            _logger.LogError("Model state is invalid: {ModelStateErrors}", ModelState.Values.SelectMany(v => v.Errors));
            return BadRequest(ModelState);
        }
        
        try
        {
            var userCheck = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == requestDto.Email);
            if (userCheck != null)
            {
                return BadRequest(new { message = "A user with this email already exists." });
            }
            
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
                _logger.LogInformation("User created successfully: {UserId}", user.Id);
                
                var roleResult = await _userManager.AddToRoleAsync(user, "User");
               
                if (roleResult.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    
                    if (roles.Count == 0)
                    {
                        _logger.LogError("Roles are null or empty for user: {UserId}", user.Id);
                        return StatusCode(500, new { message = "Roles are null or empty. Please, make new account."});
                    }

                    _logger.LogInformation("Roles assigned to user: {Roles}", string.Join(", ", roles));

                    var newUserDto = new NewUserDto
                    {
                        Id = user.Id,
                        UserName = user.Email,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Role = roles[0]
                    };
                    _logger.LogInformation("NewUserDto created: {@NewUserDto}", newUserDto);

                    return CreatedAtAction("GetById", new { userId = user.Id }, newUserDto);
                }
                else
                {
                    _logger.LogError("Failed to add user to role: {Errors}", roleResult.Errors);
                    return StatusCode(500, roleResult.Errors);
                }
                
            }
            else
            {
                _logger.LogError("Failed to create user: {Errors}", newUser.Errors);
                return StatusCode(500, newUser.Errors);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to register new user.");
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPost("staff")]
    [Authorize (Roles = "Admin")]
    public async Task<ActionResult> RegisterStaffAsync([FromBody] RegisterUserRequestDto requestDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError("Model state is invalid: {ModelStateErrors}", ModelState.Values.SelectMany(v => v.Errors));
            return BadRequest(ModelState);
        }
        
        try
        {
            var userCheck = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == requestDto.Email);
            if (userCheck != null)
            {
                return BadRequest(new { message = "A user with this email already exists." });
            }

            if (!requestDto.IsStaff)
            {
                return BadRequest(new { message = "Simple user cannot be registered as staff" });
            }
            
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
                _logger.LogInformation("User created successfully: {UserId}", user.Id);

                var roleResult = await _userManager.AddToRoleAsync(user, "Rescue Team");
               
                if (roleResult.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    
                    if (roles.Count == 0)
                    {
                        _logger.LogError("Roles are null or empty for user: {UserId}", user.Id);
                        return StatusCode(500, new { message = "Roles are null or empty. Please, make new account."});
                    }

                    _logger.LogInformation("Roles assigned to user: {Roles}", string.Join(", ", roles));

                    var newUserDto = new NewUserDto
                    {
                        Id = user.Id,
                        UserName = user.Email,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Role = roles[0]
                    };
                    _logger.LogInformation("NewUserDto created: {@NewUserDto}", newUserDto);

                    return CreatedAtAction("GetById", new { userId = user.Id }, newUserDto);
                }
                else
                {
                    _logger.LogError("Failed to add user to role: {Errors}", roleResult.Errors);
                    return StatusCode(500, roleResult.Errors);
                }
                
            }
            else
            {
                _logger.LogError("Failed to create user: {Errors}", newUser.Errors);
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
    [Authorize(Roles = "User, Rescue Team, Admin")]
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

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
    {
        User? user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email);

        if (user == null)
        {
            return Unauthorized("Invalid email or password");
        }

        if (user.UserName == null)
        {
            return BadRequest("Username is missing.");
        }
        
        var result = await _signInManager.PasswordSignInAsync(user.UserName, loginDto.Password, loginDto.RememberMe, lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            return Unauthorized("Invalid email or password");
        }
        
        var roles = await _userManager.GetRolesAsync(user);
        
        return Ok(
            new NewUserDto
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.Email,
                Role = roles.FirstOrDefault(),
                Id = user.Id
            });   
        
    }
    
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> LogoutAsync()
    {
        try
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError("Error at logout.");
            return StatusCode(500, e.Message);
        }
    }
}