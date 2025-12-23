using System;
using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.OwnsOne(x => x.OrderedItem, o =>
        {
            o.WithOwner();
            o.Property(x => x.ProductName).HasMaxLength(100);
        });
        builder.Property(x => x.Price).HasColumnType("decimal(16,2)");
    }
}
