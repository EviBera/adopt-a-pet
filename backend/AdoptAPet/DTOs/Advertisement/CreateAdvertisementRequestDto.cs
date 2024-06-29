using System.ComponentModel.DataAnnotations;
using AdoptAPet.Models;

namespace AdoptAPet.DTOs.Advertisement;

public class CreateAdvertisementRequestDto
{
    [Required]
    public int PetId { get; init; }
    [Required]
    public DateTime ExpiresAt { get; set; }
}