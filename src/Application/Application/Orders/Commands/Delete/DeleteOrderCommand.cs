using Application.Application.Base.Commands;
using MediatR;

namespace Application.Application.Orders.Commands.Delete;

public class DeleteOrderCommand : BaseDeleteCommand, IRequest<bool>
{
    public DeleteOrderCommand(Guid id) : base(id)
    {
    }
}