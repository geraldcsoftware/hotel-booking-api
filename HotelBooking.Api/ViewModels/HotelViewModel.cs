namespace HotelBooking.Api.ViewModels;

public class HotelViewModel
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public int Rating { get; set; }
    public string? Location { get; set; }
    public string? Address { get; set; }
    public string? Description { get; set; }
    public decimal PricesFrom { get; set; }
    public Dictionary<string, object> Features { get; set; } = new();
    public ICollection<RoomTypeViewModel> RoomTypes { get; set; } = new List<RoomTypeViewModel>();
}