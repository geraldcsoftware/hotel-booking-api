using HotelBooking.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBooking.Api.Data.Configurations;

public class ReservationEntityConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("Reservations");
        builder.Property(x => x.Id).IsRequired().HasMaxLength(50).IsUnicode(false);
        builder.Property(x => x.UserId).IsRequired().HasMaxLength(50).IsUnicode(false);
        builder.Property(x => x.HotelId).IsRequired().HasMaxLength(50).IsUnicode(false);
        builder.Property(x => x.Created).IsRequired().HasColumnType("datetime");
        builder.Property(x => x.RoomTypeId).IsRequired().HasMaxLength(50).IsUnicode(false);
        builder.Property(x => x.NumberOfRoomsBooked).IsRequired();
        builder.Property(x => x.Occupants).IsRequired();
        builder.Property(x => x.CheckIn).IsRequired().HasColumnType("date");
        builder.Property(x => x.CheckOut).IsRequired().HasColumnType("date");
        builder.Property(x => x.PaymentStatus).HasMaxLength(50).IsUnicode(false);

        builder.HasOne(x => x.Hotel).WithMany().HasForeignKey(x => x.HotelId);
        builder.HasOne(x => x.RoomType).WithMany(r => r.Reservations).HasForeignKey(x => x.RoomTypeId);

        builder.HasIndex(x => new { x.CheckIn, x.CheckOut, x.HotelId }).IncludeProperties(x => x.RoomTypeId!);
    }
}