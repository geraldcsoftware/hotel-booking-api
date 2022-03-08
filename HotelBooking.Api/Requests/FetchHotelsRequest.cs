using HotelBooking.Api.ViewModels;
using MediatR;

namespace HotelBooking.Api.Requests;

public record FetchHotelsRequest(int PageSize, int Page) : IRequest<PagedCollection<HotelViewModel>>;