using System.ComponentModel.DataAnnotations;

namespace AdoptAPet.DTOs.User;

public class RegisterUserRequestDto
{
    [Required]
    [MaxLength(50, ErrorMessage = "Firstname cannot be longer than 50 characters.")]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    [MaxLength(50, ErrorMessage = "Lastname cannot be longer than 50 characters.")]
    public string LastName { get; set; } = string.Empty;
    [Required]
    [MaxLength(50, ErrorMessage = "Email cannot be longer than 50 characters.")]
    [EmailAddress (ErrorMessage = "Must be valid email format.")]
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}