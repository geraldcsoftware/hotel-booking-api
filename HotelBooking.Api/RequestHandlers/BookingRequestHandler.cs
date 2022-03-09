using FluentValidation;
using FluentValidation.Results;
using HotelBooking.Api.Models;
using HotelBooking.Api.Requests;
using HotelBooking.Api.Services;
using HotelBooking.Api.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Api.RequestHandlers;

public class BookingRequestHandler : IRequestHandler<BookingRequest, ReservationViewModel>
{
    private readonly AppDbContext _appDbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<BookingRequestHandler> _logger;

    public BookingRequestHandler(AppDbContext appDbContext,
                                 IHttpContextAccessor httpContextAccessor,
                                 ILogger<BookingRequestHandler> logger)
    {
        _appDbContext = appDbContext;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<ReservationViewModel> Handle(BookingRequest request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "Anonymous";
        var hotel = await _appDbContext.Hotels
                                       .Include(x => x.Offers.Where(offer => offer.Id == request.RoomTypeId))
                                       .FirstOrDefaultAsync(hotel => hotel.Id == request.HotelId, cancellationToken);

        if (hotel is null)
        {
            _logger.LogWarning("Invalid hotel ID {HotelId} in request", request.HotelId);
            throw new ValidationException(new[] { new ValidationFailure("HotelId", "Invalid hotel ID") });
        }

        var roomOffer = hotel.Offers.FirstOrDefault(x => x.Id == request.RoomTypeId);

        if (roomOffer is null)
        {
            _logger.LogWarning("Invalid room offer ID {RoomTypeId} in request", request.RoomTypeId);
            throw new ValidationException(new[] { new ValidationFailure("RoomTypeId", "Invalid room offer ID") });
        }

        var reservation = new Reservation
        {
            CheckIn = request.CheckInDate,
            CheckOut = request.CheckOutDate,
            Hotel = hotel,
            NumberOfRoomsBooked = 1,
            PaymentStatus = "PENDING", // TODO: Change to "PAID" when payment is successful
            UserId = userId,
            RoomType = roomOffer,
            Id = Guid.NewGuid().ToString()
        };
        _appDbContext.Reservations.Add(reservation);
        await _appDbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Reservation {ReservationId} created for user {UserId}", reservation.Id, userId);

        return new()
        {
            Id = reservation.Id,
            CheckIn = reservation.CheckIn,
            CheckOut = reservation.CheckOut,
            Created = reservation.Created,
            NumberOfGuests = reservation.Occupants,
            PaymentStatus = reservation.PaymentStatus,
            Price = roomOffer.Price,
            RoomType = roomOffer.Title,
            UserId = reservation.UserId
        };
    }
}