using FluentValidation;
using HotelBooking.Api.Requests;
using HotelBooking.Api.ViewModels;
using MediatR;

namespace HotelBooking.Api.Pipelines;

public class BookingValidationPipeline : IPipelineBehavior<BookingRequest, ReservationViewModel>
{
    private readonly IValidator<BookingRequest> _validator;

    public BookingValidationPipeline(IValidator<BookingRequest> validator)
    {
        _validator = validator;
    }

    public Task<ReservationViewModel> Handle(BookingRequest request, CancellationToken cancellationToken,
                                             RequestHandlerDelegate<ReservationViewModel> next)
    {
        var result = _validator.Validate(request);
        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }

        return next();
    }
}