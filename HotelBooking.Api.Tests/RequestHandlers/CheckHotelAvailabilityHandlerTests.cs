using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using FluentAssertions;
using HotelBooking.Api.Models;
using HotelBooking.Api.RequestHandlers;
using HotelBooking.Api.Requests;
using HotelBooking.Api.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HotelBooking.Api.Tests.RequestHandlers;

public class CheckHotelAvailabilityHandlerTests
{
    public CheckHotelAvailabilityHandlerTests()
    {
    }

    [Fact]
    public void ShouldReturnAvailableRooms()
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

        var hotelRooms = new List<RoomOffer>
        {
            new RoomOffer
            {
                Id = Guid.NewGuid().ToString(),
                HotelId = fakeHotel.Id,
                Title = "Single Bed",
                Price = 100,
                MaximumOccupants = 1,
                Available = 5,
            },
            new RoomOffer
            {
                Id = Guid.NewGuid().ToString(),
                HotelId = fakeHotel.Id,
                Title = "Double Sharing",
                Price = 200,
                MaximumOccupants = 2,
                Available = 2,
            },
        };
        var reservations = new List<Reservation>
        {
            new()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
                CheckIn = DateTime.Now.AddDays(1), CheckOut = DateTime.Now.AddDays(2), HotelId = fakeHotel.Id,
                RoomTypeId = hotelRooms[0].Id
            },
            new()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
                CheckIn = DateTime.Now.AddDays(2), CheckOut = DateTime.Now.AddDays(3), HotelId = fakeHotel.Id,
                RoomTypeId = hotelRooms[0].Id
            },
            new()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
                CheckIn = DateTime.Now.AddDays(1), CheckOut = DateTime.Now.AddDays(2), HotelId = fakeHotel.Id,
                RoomTypeId = hotelRooms[1].Id
            },
            new()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
                CheckIn = DateTime.Now.AddDays(2), CheckOut = DateTime.Now.AddDays(4), HotelId = fakeHotel.Id,
                RoomTypeId = hotelRooms[1].Id
            },
        };

        var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                              .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        var dbContext = new AppDbContext(dbContextOptions);
        dbContext.Hotels.Add(fakeHotel);
        dbContext.RoomOffers.AddRange(hotelRooms);
        dbContext.Reservations.AddRange(reservations);
        dbContext.SaveChanges();

        var request = new CheckHotelAvailabilityRequest(fakeHotel.Id, DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                                                        DateOnly.FromDateTime(DateTime.Now.AddDays(3)));
        var handler = new CheckHotelAvailabilityRequestHandler(dbContext);

        // Act
        var result = handler.Handle(request, default).GetAwaiter().GetResult();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().Id.Should().Be(hotelRooms[0].Id);
    }
}