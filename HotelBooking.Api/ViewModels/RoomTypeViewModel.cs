namespace HotelBooking.Api.ViewModels;

public class RoomTypeViewModel
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public decimal Price { get; set; }
    public int MaximumOccupants { get; set; }
}