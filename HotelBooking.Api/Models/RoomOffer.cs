namespace HotelBooking.Api.Models;

public class RoomOffer
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public decimal Price { get; set; }
    public int MaximumOccupants { get; set; }
    public string? HotelId { get; set; }
    public int Available { get; set; }
    public Hotel? Hotel { get; set; }
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}