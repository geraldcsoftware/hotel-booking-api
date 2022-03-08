using System.Reflection;
using HotelBooking.Api.Requests;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Get Hotels api. Parameters -> pageSize, page
app.MapGet("/api/v1/hotels", async (int? pageSize, int? page, IMediator mediator) =>
    {
        var fetchRequest = new FetchHotelsRequest(pageSize ?? 10, page ?? 1);
        var response = await mediator.Send(fetchRequest).ConfigureAwait(false);
        return response;
    })
   .WithName("FetchHotels");

app.Run();