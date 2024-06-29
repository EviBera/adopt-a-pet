using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AdoptAPet.Models;

public class User : IdentityUser
{
    [MaxLength(50)]
    public string FirstName { get; set; } = String.Empty;
    [MaxLength(50)]
    public string LastName { get; set; } = String.Empty;

    public virtual ICollection<Application> Applications { get; set; } = new HashSet<Application>();
}