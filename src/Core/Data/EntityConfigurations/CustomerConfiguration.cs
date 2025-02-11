using Core.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.EntityConfigurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers").HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName(nameof(Customer.Id)).HasColumnType("uuid");
        builder.Property(c => c.Name).HasColumnName(nameof(Customer.Name)).HasColumnType("citext").HasMaxLength(100).IsRequired();
        builder.Property(c => c.LastName).HasColumnName(nameof(Customer.LastName)).HasColumnType("citext").HasMaxLength(100).IsRequired();
        builder.Property(c => c.Address).HasColumnName(nameof(Customer.Address)).HasColumnType("citext").HasMaxLength(100).IsRequired();
        builder.Property(c => c.PostalCode).HasColumnName(nameof(Customer.PostalCode)).HasColumnType("citext").HasMaxLength(100).IsRequired();
    }
}