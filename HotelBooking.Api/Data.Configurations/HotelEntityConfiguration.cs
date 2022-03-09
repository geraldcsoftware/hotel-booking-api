using HotelBooking.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBooking.Api.Data.Configurations;

public class HotelEntityConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.ToTable("Hotels");
        builder.Property(x => x.Id).IsRequired().HasMaxLength(50).IsUnicode(false);
        builder.Property(x => x.Address).IsRequired().HasMaxLength(500).IsUnicode();
        builder.Property(x => x.Description).IsRequired(false).HasMaxLength(1000).IsUnicode();
        builder.Property(x => x.Location).IsRequired().HasMaxLength(100).IsUnicode();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100).IsUnicode();
        builder.Property(x => x.Rating).IsRequired();
        builder.HasMany(x => x.Offers).WithOne().HasForeignKey(offer => offer.HotelId);
        builder.HasMany(x => x.Features).WithOne().HasForeignKey(feature => feature.HotelId);

        builder.HasIndex(x => x.Rating);
        builder.HasIndex(x => x.Location);
    }
}