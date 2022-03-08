namespace HotelBooking.Api.ViewModels;

public record PagedCollection<T>(ICollection<T> Items, int PageSize = 1, int TotalItems = 0) where T : class;