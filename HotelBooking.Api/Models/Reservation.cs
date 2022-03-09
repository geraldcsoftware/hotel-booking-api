using HotelBooking.Api.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Api.Models;

[EntityTypeConfiguration(typeof(ReservationEntityConfiguration))]
public class Reservation
{
    public string? Id { get; set; }
    public string? UserId { get; set; }
    public DateTime Created { get; set; }
    public string? HotelId { get; set; }
    public string? RoomTypeId { get; set; }
    public int NumberOfRoomsBooked { get; set; }
    public int Occupants { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public string? PaymentStatus { get; set; }
    public Hotel? Hotel { get; set; }
    public RoomOffer? RoomType { get; set; }
}