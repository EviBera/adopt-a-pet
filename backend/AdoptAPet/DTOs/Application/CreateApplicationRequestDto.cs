namespace AdoptAPet.DTOs.Application;

public class CreateApplicationRequestDto
{
    public string UserId { get; set; } = string.Empty;
    public int AdvertisementId { get; set; }
}