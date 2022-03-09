using HotelBooking.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBooking.Api.Data.Configurations;

public class FeatureDescriptionConfiguration : IEntityTypeConfiguration<FeatureDescription>
{
    public void Configure(EntityTypeBuilder<FeatureDescription> builder)
    {
        builder.ToTable("HotelFeatures");

        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.FeatureName).IsRequired();
        builder.Property(x => x.Description);
        builder.Property(x => x.Value).IsRequired().HasColumnType("bson");
        builder.Property(x => x.HotelId).IsRequired();
        
    }
}