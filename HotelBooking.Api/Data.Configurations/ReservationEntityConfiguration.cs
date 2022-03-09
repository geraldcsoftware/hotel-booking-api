using HotelBooking.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBooking.Api.Data.Configurations;

public class ReservationEntityConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("Reservations");
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.HotelId).IsRequired();
        builder.Property(x => x.RoomTypeId).IsRequired();
        builder.Property(x => x.NumberOfRoomsBooked).IsRequired();
        builder.Property(x => x.CheckIn).IsRequired();
        builder.Property(x => x.CheckOut).IsRequired();
        builder.Property(x => x.PaymentStatus);

        builder.HasOne(x => x.Hotel).WithMany().HasForeignKey(x => x.HotelId);
        builder.HasOne(x => x.RoomType).WithMany(r => r.Reservations).HasForeignKey(x => x.RoomTypeId);

        builder.HasIndex(x => new { x.CheckIn, x.CheckOut, x.HotelId }).IncludeProperties(x => x.RoomTypeId!);
    }
}