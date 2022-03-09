using System;
using System.Collections.Generic;
using System.Security.Claims;
using Bogus;
using HotelBooking.Api.Models;
using HotelBooking.Api.RequestHandlers;
using HotelBooking.Api.Requests;
using HotelBooking.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace HotelBooking.Api.Tests.RequestHandlers;

public class BookingRequestHandlerTests
{
    [Fact]
    public void BookingRequestHandler_ShouldReturnBookingWhenRequestIsValid()
    {
        // Arrange
        var hotelFaker = new Faker<Hotel>()
                        .RuleFor(h => h.Id, f => f.Random.Guid().ToString())
                        .RuleFor(h => h.Name, f => f.Company.CompanyName())
                        .RuleFor(h => h.Address, f => f.Address.StreetAddress())
                        .RuleFor(h => h.Description, f => f.Lorem.Paragraph())
                        .RuleFor(h => h.Location, f => f.Address.City())
                        .RuleFor(h => h.Rating, f => f.Random.Int(1, 5));
        var fakeHotel = hotelFaker.Generate();

        var hotelRoomOffer = new RoomOffer
        {
            Id = Guid.NewGuid().ToString(),
            HotelId = fakeHotel.Id,
            Title = "Single Bed",
            Price = 100,
            MaximumOccupants = 1,
            NumberOfRooms = 5,
        };


        var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                              .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        var dbContext = new AppDbContext(dbContextOptions);
        dbContext.Hotels.Add(fakeHotel);
        dbContext.RoomOffers.Add(hotelRoomOffer);
        dbContext.SaveChanges();

        var bookingRequest = new BookingRequest
        {
            CheckInDate = DateTime.Now,
            CheckOutDate = DateTime.Now.AddDays(1),
            RoomTypeId = hotelRoomOffer.Id,
            HotelId = fakeHotel.Id,
            NumberOfGuests = 1
        };
        Mock<IHttpContextAccessor> mockHttpContextAccessor = new();
        mockHttpContextAccessor.Setup(x => x.HttpContext)
                               .Returns(new DefaultHttpContext()
                                {
                                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                                    {
                                        new Claim(ClaimTypes.Name, "TestUser")
                                    }))
                                });
        var bookingRequestHandler = new BookingRequestHandler(dbContext,
                                                              mockHttpContextAccessor.Object,
                                                              new NullLogger<BookingRequestHandler>());

        // Act
        var booking = bookingRequestHandler.Handle(bookingRequest, default).GetAwaiter().GetResult();

        // Assert
        Assert.NotNull(booking);
    }
}