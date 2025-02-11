using Application.Application.Base.Commands;
using Core.Domain.Audit;
using Core.Domain.Base;
using MediatR;

namespace Application.Application.Base.Behaviors;

public class AuditLogBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IAuditLogRepository _auditLogRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AuditLogBehavior(IAuditLogRepository auditLogRepository, IUnitOfWork unitOfWork)
    {
        _auditLogRepository = auditLogRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();

        if (request is IAuditLogRequest auditRequest)
        {
            var auditLog = new AuditLog(
                auditRequest.EntityName,
                auditRequest.EntityId,
                auditRequest.ActionType,
                auditRequest.OldValue,
                auditRequest.NewValue,
                "Unknown User"); // Can be added UserContext that holds details about request maker user for this field.
            
            await _auditLogRepository.CreateAsync(auditLog, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }

        return response;
    }
}