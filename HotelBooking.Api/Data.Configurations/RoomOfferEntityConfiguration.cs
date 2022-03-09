using HotelBooking.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBooking.Api.Data.Configurations;

public class RoomOfferEntityConfiguration : IEntityTypeConfiguration<RoomOffer>
{
    public void Configure(EntityTypeBuilder<RoomOffer> builder)
    {
        builder.ToTable("RoomOffers");

        builder.Property(x => x.Available).IsRequired();
        builder.Property(x => x.HotelId).IsRequired();
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.MaximumOccupants).IsRequired();
        builder.Property(x => x.Price).IsRequired();
        builder.Property(x => x.Title).IsRequired();

        builder.HasOne(x => x.Hotel).WithMany().HasForeignKey(x => x.HotelId);
    }
}