namespace HotelBooking.Api.Models;

public class Hotel
{
    public string? Name { get; set; }
    public int Rating { get; set; }
    public string? Location { get; set; }
    public string? Id { get; set; }
    public string? Address { get; set; }
    public string? Description { get; set; }
    public ICollection<FeatureDescription> Features { get; } = new List<FeatureDescription>();
    public ICollection<RoomOffer> Offers { get; } = new List<RoomOffer>();
}