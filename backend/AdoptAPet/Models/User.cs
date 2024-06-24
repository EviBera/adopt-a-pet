using Microsoft.AspNetCore.Identity;

namespace AdoptAPet.Models;

public class User : IdentityUser
{
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
}