using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using HotelBooking.Api.Requests;
using HotelBooking.Api.Services;
using HotelBooking.Api.Validators;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DbConnection");
if (string.IsNullOrEmpty(connectionString)) throw new("Connection string not properly configured");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddStackExchangeRedisCache(options =>
                                                options.Configuration =
                                                    builder.Configuration.GetConnectionString("Redis"));
builder.Services.AddTransient<ISystemCache, RedisCacheWrapper>();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddFluentValidation();
builder.Services.AddTransient<IValidator<BookingRequest>, BookingRequestValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Get Hotels api. Parameters -> pageSize, page, location, minimumRating
app.MapGet("/api/v1/hotels", async (int? pageSize, int? page, string? location, int? minRating, IMediator mediator) =>
    {
        var fetchRequest = new FetchHotelsRequest(pageSize ?? 10, page ?? 1, location, minRating);
        var response = await mediator.Send(fetchRequest).ConfigureAwait(false);
        return response;
    })
   .WithDisplayName("Fetch Hotels")
   .WithGroupName("Hotels");

app.MapGet("/api/v1/hotels/{hotelId}", async (string hotelId, IMediator mediator) =>
    {
        var result = await mediator.Send(new FindHotelByIdRequest(hotelId));
        return result switch
        {
            null => Results.NotFound(),
            { }  => Results.Ok(result)
        };
    })
   .WithDisplayName("Find Hotel By Id")
   .WithGroupName("Hotels");


app.MapGet("/api/v1/hotels/{hotelId}/availability",
           async (string hotelId, DateOnly? checkIn, DateOnly? checkOut, IMediator mediator) =>
           {
               var checkInDate = checkIn   ?? DateOnly.FromDateTime(DateTime.Today.AddDays(1));
               var checkOutDate = checkOut ?? DateOnly.FromDateTime(DateTime.Today.AddDays(2));
               var result = await mediator.Send(new CheckHotelAvailabilityRequest(hotelId, checkInDate, checkOutDate));
               return Results.Ok(result);
           })
   .WithDisplayName("Check Hotel Availability")
   .WithGroupName("Hotels");

app.MapPost("/api/v1/bookings", async (BookingRequest model, IMediator mediator) =>
    {
        try
        {
            var result = await mediator.Send(model);
            return result switch
            {
                null => Results.BadRequest(),
                { }  => Results.Created($"/api/v1/reservations/{result.Id}", result)
            };
        }
        catch (ValidationException exception)
        {
            var problemDetails = ToProblemDetails(exception);
            return Results.BadRequest(problemDetails);
        }
    })
    //.RequireAuthorization()
   .WithDisplayName("Book Hotel")
   .WithGroupName("Bookings");


app.Run();

ValidationProblemDetails ToProblemDetails(ValidationException validationException)
{
    var validationProblemDetails = new ValidationProblemDetails
    {
        Title = "Validation Error",
        Detail = "See errors for details."
    };
    foreach (var error in validationException.Errors)
    {
        validationProblemDetails.Errors.Add(error.PropertyName, new[] { error.ErrorMessage });
    }

    return validationProblemDetails;
}