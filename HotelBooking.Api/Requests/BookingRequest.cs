using HotelBooking.Api.ViewModels;
using MediatR;

namespace HotelBooking.Api.Requests;

public class BookingRequest : IRequest<ReservationViewModel>
{
    public string? HotelId { get; set; }
    public string? RoomTypeId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int NumberOfGuests { get; set; }
}