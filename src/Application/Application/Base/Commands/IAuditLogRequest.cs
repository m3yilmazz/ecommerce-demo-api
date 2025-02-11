namespace Application.Application.Base.Commands;

public interface IAuditLogRequest
{
    string EntityName { get; }
    Guid EntityId { get; }
    string ActionType { get; }
    string OldValue { get; }
    string NewValue { get; }
}