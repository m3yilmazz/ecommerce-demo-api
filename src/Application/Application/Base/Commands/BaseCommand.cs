namespace Application.Application.Base.Commands;

public abstract class BaseCommand
{
    public Guid Id { get; }

    protected BaseCommand(Guid id)
    {
        Id = id;
    }
}