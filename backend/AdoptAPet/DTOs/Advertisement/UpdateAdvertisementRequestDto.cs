namespace AdoptAPet.DTOs.Advertisement;

public class UpdateAdvertisementRequestDto
{
    public DateTime? ExpiresAt { get; set; }
    public ICollection<Models.Application>? Applications { get; set; }
}