using Core.Domain.Base;
using Dawn;

namespace Core.Domain.Audit;

public class AuditLog : AggregateRoot
{
    public string EntityName { get; init; } // e.g., Customer, Order
    public Guid EntityId { get; init; }
    public string ActionType { get; init; } // e.g., Create, Update, Delete
    public string OldValue { get; init; }
    public string NewValue { get; init; }
    public string ChangedBy { get; init; } // The user who made the change

    public AuditLog()
    {

    }

    public AuditLog(
        string entityName,
        Guid entityId,
        string actionType,
        string oldValue,
        string newValue,
        string changedBy)
    {
        Guard.Argument(entityName, nameof(entityName)).NotNull().NotEmpty().NotWhiteSpace();
        Guard.Argument(entityId, nameof(entityId)).NotDefault();
        Guard.Argument(actionType, nameof(actionType)).NotNull().NotEmpty().NotWhiteSpace();
        Guard.Argument(oldValue, nameof(oldValue)).NotNull().NotEmpty().NotWhiteSpace();
        Guard.Argument(newValue, nameof(newValue)).NotNull().NotEmpty().NotWhiteSpace();
        Guard.Argument(changedBy, nameof(changedBy)).NotNull().NotEmpty().NotWhiteSpace();

        EntityName = entityName;
        EntityId = entityId;
        ActionType = actionType;
        OldValue = oldValue;
        NewValue = newValue;
        ChangedBy = changedBy;
    }
}