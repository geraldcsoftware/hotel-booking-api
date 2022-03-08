namespace HotelBooking.Api.Models;

public class RoomOffer
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public decimal Price { get; set; }
    public int MaximumOccupants { get; set; }
    public string? HotelId { get; set; }
    public int Available { get; set; }
}