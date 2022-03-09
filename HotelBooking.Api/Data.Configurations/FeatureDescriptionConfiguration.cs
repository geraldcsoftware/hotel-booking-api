using HotelBooking.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBooking.Api.Data.Configurations;

public class FeatureDescriptionConfiguration : IEntityTypeConfiguration<FeatureDescription>
{
    public void Configure(EntityTypeBuilder<FeatureDescription> builder)
    {
        builder.ToTable("HotelFeatures");

        builder.Property(x => x.Id).IsRequired().HasMaxLength(50).IsUnicode(false);
        builder.Property(x => x.FeatureName).IsRequired().HasMaxLength(50).IsUnicode(false);
        builder.Property(x => x.Description).HasMaxLength(500).IsUnicode(false);
        builder.Property(x => x.Value).IsRequired().HasMaxLength(8000).IsUnicode();
        builder.Property(x => x.HotelId).IsRequired().IsRequired().HasMaxLength(50).IsUnicode(false);;
        
    }
}