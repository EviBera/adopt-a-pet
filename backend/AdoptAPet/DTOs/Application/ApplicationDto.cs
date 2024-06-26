namespace AdoptAPet.DTOs.Application;

public class ApplicationDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int AdvertisementId { get; set; }
    public bool? IsAccepted { get; set; } = null;
}