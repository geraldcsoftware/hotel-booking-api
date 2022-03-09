using HotelBooking.Api.Models;
using HotelBooking.Api.Requests;
using HotelBooking.Api.Services;
using HotelBooking.Api.ViewModels;
using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Api.RequestHandlers;

public class CheckHotelAvailabilityRequestHandler :
    IRequestHandler<CheckHotelAvailabilityRequest, IReadOnlyCollection<RoomTypeViewModel>>
{
    private readonly AppDbContext _dbContext;

    public CheckHotelAvailabilityRequestHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<RoomTypeViewModel>> Handle(CheckHotelAvailabilityRequest request,
                                                                     CancellationToken cancellationToken)
    {
        var checkInDate = request.CheckIn.ToDateTime(TimeOnly.MinValue);
        var checkOutDate = request.CheckOut.ToDateTime(TimeOnly.MinValue);

        var clashingReservationPredicate = PredicateBuilder.New<Reservation>(reservation =>
                                                                                 reservation.CheckIn  <= checkOutDate ||
                                                                                 reservation.CheckOut >= checkInDate);
        var availableRoomsPredicate = PredicateBuilder.New<RoomOffer>(
                                                                      offer =>
                                                                          offer.Reservations
                                                                             .Count(clashingReservationPredicate) <
                                                                          offer.Available);

        var availableRoomTypes = await _dbContext.Hotels
                                                 .Include(h => h.Offers)
                                                 .ThenInclude(o => o.Reservations)
                                                 .AsNoTracking()
                                                 .Where(h => h.Id == request.HotelId)
                                                 .SelectMany(h => h.Offers)
                                                 .Where(availableRoomsPredicate)
                                                 .Distinct()
                                                 .Select(x => new RoomTypeViewModel
                                                  {
                                                      Id = x.Id,
                                                      Price = x.Price,
                                                      Title = x.Title,
                                                      MaximumOccupants = x.MaximumOccupants
                                                  })
                                                 .ToListAsync(cancellationToken);

        return availableRoomTypes;
    }
}