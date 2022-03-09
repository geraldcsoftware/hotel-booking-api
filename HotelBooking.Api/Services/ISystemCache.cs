namespace HotelBooking.Api.Services;

public interface ISystemCache
{
    Task<T?> Get<T>(string key);
    Task Add<T>(string key, T value);
}