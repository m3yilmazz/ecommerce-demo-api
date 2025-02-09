namespace Application.Application.Base.Commands;

public abstract class BaseDeleteCommand : BaseCommand
{
    protected BaseDeleteCommand(Guid id) : base(id)
    {
    }
}