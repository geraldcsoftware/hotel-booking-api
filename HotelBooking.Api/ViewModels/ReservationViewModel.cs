namespace HotelBooking.Api.ViewModels;

public class ReservationViewModel
{
    public string? Id { get; set; }
    public string? UserId { get; set; }
    public string? RoomType { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int NumberOfGuests { get; set; }
    public decimal Price { get; set; }
    public string? PaymentStatus { get; set; }
    public DateTime Created { get; set; }
}