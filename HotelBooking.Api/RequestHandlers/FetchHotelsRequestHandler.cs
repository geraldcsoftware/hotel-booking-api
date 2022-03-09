using HotelBooking.Api.Models;
using HotelBooking.Api.Requests;
using HotelBooking.Api.Services;
using HotelBooking.Api.ViewModels;
using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Api.RequestHandlers;

public class FetchHotelsRequestHandler : IRequestHandler<FetchHotelsRequest, PagedCollection<HotelViewModel>>
{
    private readonly IDbContext _dbContext;

    public FetchHotelsRequestHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedCollection<HotelViewModel>> Handle(FetchHotelsRequest request,
                                                              CancellationToken cancellationToken)
    {
        var predicate = PredicateBuilder.New<Hotel>();

        if (!string.IsNullOrEmpty(request.Location))
            predicate = predicate.And(hotel => hotel.Location != null && hotel.Location.Contains(request.Location));
        // TODO: Use geo-spatial datatype for Location, and filter with distance query instead.

        if (request.MinimumRating.HasValue)
            predicate = predicate.And(hotel => hotel.Rating >= request.MinimumRating);

        var count = await _dbContext.Hotels.CountAsync(predicate, cancellationToken);
        var hotels = await _dbContext.Hotels
                                     .Where(predicate)
                                     .OrderByDescending(h => h.Rating)
                                     .Skip(request.PageSize * (request.Page - 1))
                                     .Take(request.PageSize)
                                     .Select(h => new HotelViewModel
                                      {
                                          Address = h.Address,
                                          Description = h.Description,
                                          Id = h.Id,
                                          Location = h.Location,
                                          Rating = h.Rating,
                                          Name = h.Name,
                                          PricesFrom = h.Offers.Min(offer => offer.Price),
                                          RoomTypes = h.Offers.OrderByDescending(x => x.Price)
                                                       .Select(x => new RoomTypeViewModel
                                                        {
                                                            Id = x.Id,
                                                            Price = x.Price,
                                                            Title = x.Title,
                                                            MaximumOccupants = x.MaximumOccupants
                                                        }).ToList()
                                      })
                                     .ToListAsync(cancellationToken);
        return new(hotels, request.PageSize, count);
    }
}