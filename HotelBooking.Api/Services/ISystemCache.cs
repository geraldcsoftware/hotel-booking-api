namespace HotelBooking.Api.Services;

public interface ISystemCache
{
    Task<bool> TryGet<T>(string key, out T? value);
    Task Add<T>(string key, T value);
}