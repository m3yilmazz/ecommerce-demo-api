using Core.Domain.Base;

namespace Core.Domain.Audit;

public interface IAuditLogRepository : IEfRepository<AuditLog>
{
}