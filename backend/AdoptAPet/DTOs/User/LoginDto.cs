using System.ComponentModel.DataAnnotations;

namespace AdoptAPet.DTOs.User;

public class LoginDto
{
    [Required] public string Email { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
    public bool RememberMe { get; set; } 
}