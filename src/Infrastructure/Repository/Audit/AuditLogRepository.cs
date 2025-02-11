using Core.Data;
using Core.Domain.Audit;
using Infrastructure.Repository.Base;

namespace Infrastructure.Repository.Audit;

public class AuditLogRepository : EfBaseRepository<AuditLog>, IAuditLogRepository
{
    public AuditLogRepository(Context context) : base(context)
    {
    }
}