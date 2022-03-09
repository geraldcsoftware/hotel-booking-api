using HotelBooking.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBooking.Api.Data.Configurations;

public class HotelEntityConfiguration: IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Address);
        builder.Property(x => x.Description);
        builder.Property(x => x.Location);
        builder.Property(x => x.Name);
        builder.Property(x => x.Rating);
        builder.Property(x => x.Features);
        builder.HasMany(x => x.Offers).WithOne().HasForeignKey(offer => offer.HotelId);
    }
}