using HotelBooking.Api.ViewModels;
using MediatR;

namespace HotelBooking.Api.Requests;

public record FetchHotelsRequest(int PageSize, int Page, string? Location, int? MinimumRating) : IRequest<PagedCollection<HotelViewModel>>;