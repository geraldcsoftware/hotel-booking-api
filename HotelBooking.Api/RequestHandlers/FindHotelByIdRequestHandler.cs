using HotelBooking.Api.Requests;
using HotelBooking.Api.Services;
using HotelBooking.Api.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Api.RequestHandlers;

public class FindHotelByIdRequestHandler : IRequestHandler<FindHotelByIdRequest, HotelViewModel?>
{
    private readonly IDbContext _dbContext;
    private readonly ISystemCache _systemCache;

    public FindHotelByIdRequestHandler(IDbContext dbContext, ISystemCache systemCache)
    {
        _dbContext = dbContext;
        _systemCache = systemCache;
    }

    public async Task<HotelViewModel?> Handle(FindHotelByIdRequest request, CancellationToken cancellationToken)
    {
        var cacheKey = $"Hotel{request.HotelId}";
        if (await _systemCache.TryGet<HotelViewModel>(cacheKey, out var hotelViewModel))
            return hotelViewModel;

        hotelViewModel = await _dbContext.Hotels
                                         .Where(hotel => hotel.Id == request.HotelId)
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
                                         .FirstOrDefaultAsync(cancellationToken);
        if (hotelViewModel is not null)
            await _systemCache.Add(cacheKey, hotelViewModel);
        
        return hotelViewModel;
    }
}