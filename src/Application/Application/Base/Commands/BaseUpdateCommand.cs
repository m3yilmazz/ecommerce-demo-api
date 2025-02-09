namespace Application.Application.Base.Commands;

public abstract class BaseUpdateCommand : BaseCommand
{
    protected BaseUpdateCommand(Guid id) : base(id)
    {
    }
}