using System;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(x => x.Price).HasColumnType("decimal(16,2)");
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);

        builder.Property(x => x.Brand).HasConversion(
            p => p.ToString(),
            p => (Brand)Enum.Parse(typeof(Brand), p)
        );

        builder.Property(x => x.Type).HasConversion(
            p => p.ToString(),
            p => (Core.Entities.Type)Enum.Parse(typeof(Core.Entities.Type), p)
        );

        builder.Property(x => x.ArrivalDate).HasConversion(
            d => d.ToUniversalTime(),
            d => DateTime.SpecifyKind(d, DateTimeKind.Utc)
        );

        builder.HasMany(x => x.ProductPictures).WithOne().OnDelete(DeleteBehavior.Cascade);
    }
}
