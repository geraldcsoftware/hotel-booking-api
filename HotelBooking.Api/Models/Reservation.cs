namespace HotelBooking.Api.Models;

public class Reservation
{
    public string? Id { get; set; }
    public string? UserId { get; set; }
    public string? HotelId { get; set; }
    public string? RoomTypeId { get; set; }
    public int NumberOfRoomsBooked { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public string? PaymentStatus { get; set; }
}