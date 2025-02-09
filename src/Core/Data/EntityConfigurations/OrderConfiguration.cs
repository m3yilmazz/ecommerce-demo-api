using Core.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.EntityConfigurations;
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders").HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName(nameof(Order.Id)).HasColumnType("uuid").ValueGeneratedOnAdd();
        builder.Property(s => s.OrderDate).HasColumnName(nameof(Order.OrderDate)).IsRequired();
        builder.Property(s => s.TotalPrice).HasColumnName(nameof(Order.TotalPrice)).HasColumnType("double precision").IsRequired();

        builder.HasOne(h => h.Customer).WithMany(w => w.Orders).HasForeignKey(fk => fk.CustomerId).OnDelete(DeleteBehavior.SetNull);

        builder.OwnsMany(s => s.Items, o =>
        {
            o.ToTable("Items").HasKey(k => k.Id);
            o.Property(k => k.Id).HasColumnName(nameof(Item.Id)).HasColumnType("uuid").ValueGeneratedOnAdd();
            o.Property(k => k.QuantityOfProduct).HasColumnName(nameof(Item.QuantityOfProduct)).HasColumnType("integer").IsRequired();

            o.HasOne(i => i.Product).WithMany().HasForeignKey(i => i.ProductId).IsRequired().OnDelete(DeleteBehavior.SetNull);
            o.Navigation(n => n.Product).AutoInclude();
        });
    }
}