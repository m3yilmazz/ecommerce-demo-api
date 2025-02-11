using Core.Domain.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.EntityConfigurations;
public class AuditLogConfiguration
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs").HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName(nameof(AuditLog.Id)).HasColumnType("uuid").ValueGeneratedOnAdd();
        builder.Ignore(s => s.UpdatedAt);
        builder.Property(c => c.EntityName).HasColumnName(nameof(AuditLog.EntityName)).HasColumnType("citext").IsRequired();
        builder.Property(c => c.EntityId).HasColumnName(nameof(AuditLog.EntityId)).HasColumnType("uuid").IsRequired();
        builder.Property(c => c.ActionType).HasColumnName(nameof(AuditLog.ActionType)).HasColumnType("citext").IsRequired();
        builder.Property(c => c.OldValue).HasColumnName(nameof(AuditLog.OldValue)).HasColumnType("citext");
        builder.Property(c => c.NewValue).HasColumnName(nameof(AuditLog.NewValue)).HasColumnType("citext").IsRequired();
        builder.Property(c => c.ChangedBy).HasColumnName(nameof(AuditLog.ChangedBy)).HasColumnType("citext").IsRequired();
    }
}