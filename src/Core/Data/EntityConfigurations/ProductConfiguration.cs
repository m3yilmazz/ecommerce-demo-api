using Core.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.EntityConfigurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products").HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName(nameof(Product.Id)).HasColumnType("uuid").ValueGeneratedOnAdd();
        builder.Property(s => s.Name).HasColumnName(nameof(Product.Name)).HasColumnType("citext").HasMaxLength(100).IsRequired();
        builder.Property(s => s.Price).HasColumnName(nameof(Product.Price)).HasColumnType("double precision").IsRequired();
    }
}