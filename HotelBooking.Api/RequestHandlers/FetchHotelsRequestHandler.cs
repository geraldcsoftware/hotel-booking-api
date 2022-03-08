using HotelBooking.Api.Requests;
using HotelBooking.Api.Services;
using HotelBooking.Api.ViewModels;
using MediatR;

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
        var query = _dbContext.Hotels;
        var hotels = query
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
                         // PricesFrom
                         // Features = {  }
                         // RoomTypeViewModels = {  }
                     })
                    .ToList();
        return new(hotels, request.PageSize, query.Count());
    }
}