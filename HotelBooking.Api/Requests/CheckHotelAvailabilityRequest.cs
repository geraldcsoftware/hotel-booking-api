using HotelBooking.Api.ViewModels;
using MediatR;

namespace HotelBooking.Api.Requests;

public record CheckHotelAvailabilityRequest(string? HotelId, DateOnly CheckIn, DateOnly CheckOut) :
    IRequest<IReadOnlyCollection<RoomTypeViewModel>>;