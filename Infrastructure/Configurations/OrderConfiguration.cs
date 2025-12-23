using System;
using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsOne(x => x.ShippingAddress, o => {
            o.WithOwner();
            o.Property(x => x.City).HasMaxLength(100);
            o.Property(x => x.State).HasMaxLength(100);
            o.Property(x => x.PostalCode).HasMaxLength(100);
            o.Property(x => x.Country).HasMaxLength(100);
        });
        
        builder.OwnsOne(x => x.PaymentSummary, o =>
        {
            o.WithOwner();
            o.Property(x => x.Brand).HasMaxLength(100);
        });
        
        builder.Property(x => x.Status).HasConversion(
            o => o.ToString(),
            o => (OrderStatus)Enum.Parse(typeof(OrderStatus), o)
        );

        builder.Property(x => x.Subtotal).HasColumnType("decimal(16,2)");
        builder.Property(x => x.Total).HasColumnType("decimal(16,2)");

        builder.HasMany(x => x.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
        builder.Property(x => x.OrderDate).HasConversion(
            d => d.ToUniversalTime(),
            d => DateTime.SpecifyKind(d, DateTimeKind.Utc)
        );

        builder.Property(x => x.BuyerEmail).HasMaxLength(100);
    }
}
