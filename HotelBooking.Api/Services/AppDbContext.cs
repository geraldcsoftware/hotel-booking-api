using HotelBooking.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Api.Services;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public AppDbContext()
    {
    }

    public virtual DbSet<Reservation> Reservations => Set<Reservation>();
    public virtual DbSet<Hotel> Hotels => Set<Hotel>();
    public virtual DbSet<RoomOffer> RoomOffers => Set<RoomOffer>();
}