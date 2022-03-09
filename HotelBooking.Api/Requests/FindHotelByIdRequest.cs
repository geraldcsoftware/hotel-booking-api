using HotelBooking.Api.ViewModels;
using MediatR;

namespace HotelBooking.Api.Requests;

public record FindHotelByIdRequest(string HotelId) : IRequest<HotelViewModel?>;