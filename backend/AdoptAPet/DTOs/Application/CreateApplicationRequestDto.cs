using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace AdoptAPet.DTOs.Application;

public class CreateApplicationRequestDto
{
    [Required]
    public string UserId { get; set; } = string.Empty;
    [Required]
    public int AdvertisementId { get; set; }
}