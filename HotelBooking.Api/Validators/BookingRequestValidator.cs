using System.Data;
using FluentValidation;
using HotelBooking.Api.Requests;
using HotelBooking.Api.ViewModels;

namespace HotelBooking.Api.Validators;

public class BookingRequestValidator : AbstractValidator<BookingRequest>
{
    public BookingRequestValidator()
    {
        RuleFor(x => x.HotelId).NotEmpty();
        RuleFor(x => x.CheckInDate).NotEmpty().LessThan(x => x.CheckOutDate);
        RuleFor(x => x.CheckOutDate).NotEmpty();
        RuleFor(x => x.RoomTypeId).NotEmpty();
        RuleFor(x => x.NumberOfGuests).GreaterThan(0);
    }
}