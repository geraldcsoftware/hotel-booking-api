using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using HotelBooking.Api.Models;
using HotelBooking.Api.RequestHandlers;
using HotelBooking.Api.Requests;
using HotelBooking.Api.Services;
using HotelBooking.Api.ViewModels;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace HotelBooking.Api.Tests.RequestHandlers;

public class FindHotelByIdRequestHandlerTests
{
    private readonly Mock<ISystemCache> _systemCacheMock = new(MockBehavior.Strict);
    private readonly Mock<AppDbContext> _dbContextMock = new(MockBehavior.Strict);

    [Fact]
    public void WhenHotelExistsInCache_ShouldLoadFromCache()
    {
        // Arrange
        var fakeResult = new Faker<HotelViewModel>().Generate();
        _systemCacheMock.Setup(x => x.Get<HotelViewModel>(It.IsAny<string>()))
                        .ReturnsAsync(fakeResult)
                        .Verifiable();
        _dbContextMock.Setup(x => x.Hotels).Verifiable();
        var handler = new FindHotelByIdRequestHandler(_dbContextMock.Object, _systemCacheMock.Object);

        var request = new FindHotelByIdRequest("1234");

        // Act
        var result = handler.Handle(request, default).GetAwaiter().GetResult();

        // Assert
        _systemCacheMock.Verify(x => x.Get<HotelViewModel>(It.IsAny<string>()), Times.Once);
        _dbContextMock.Verify(x => x.Hotels, Times.Never);
        result.Should().Be(fakeResult);
    }


    [Fact]
    public void WhenHotelExistsInDatabase_ShouldUpdateCache()
    {
        // Arrange
        var fakeResult = new Faker<Hotel>()
                        .RuleFor(x => x.Id, "1234")
                        .RuleFor(x => x.Offers, new Faker<RoomOffer>().Generate(5))
                        .Generate();
        _systemCacheMock.Setup(x => x.Get<HotelViewModel>(It.IsAny<string>()))
                        .ReturnsAsync((HotelViewModel?)null);
        _systemCacheMock.Setup(x => x.Add(It.IsAny<string>(), It.IsAny<HotelViewModel>())).Returns(Task.CompletedTask).Verifiable();
        _dbContextMock.Setup(x => x.Hotels).ReturnsDbSet(new [] { fakeResult });
        var handler = new FindHotelByIdRequestHandler(_dbContextMock.Object, _systemCacheMock.Object);
        var request = new FindHotelByIdRequest("1234");

        // Act
        var result = handler.Handle(request, default).GetAwaiter().GetResult();

        // Assert
        _systemCacheMock.Verify(x => x.Get<HotelViewModel>(It.IsAny<string>()), Times.Once);
        _dbContextMock.Verify(x => x.Hotels, Times.Once);
        _systemCacheMock.Verify(x => x.Add(It.IsAny<string>(), It.IsAny<HotelViewModel>()), Times.Once);
        result.Should().NotBeNull();
    }
}