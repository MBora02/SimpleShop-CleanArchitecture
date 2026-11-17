using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleShop.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150);
            builder.Property(p => p.Description)
                .HasMaxLength(500);
            builder.Property(p => p.Price)
                .HasPrecision(18, 2)
                .IsRequired();
            builder.Property(p => p.Stock)
                .IsRequired();
            builder.HasQueryFilter(p => !p.IsDeleted);
        }
    }
}
