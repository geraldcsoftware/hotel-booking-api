using HotelBooking.Api.Models;

namespace HotelBooking.Api.Services;

public interface IDbContext
{
    IQueryable<Hotel> Hotels { get; }
    IQueryable<RoomOffer> RoomOffers { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}