using System.ComponentModel.DataAnnotations;

namespace AdoptAPet.DTOs.Application;

public class UpdateApplicationRequestDto
{
    [Required]
    public bool IsAccepted { get; set; }
}