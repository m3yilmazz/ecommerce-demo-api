using Application.Application.Base.Commands;
using MediatR;

namespace Application.Application.Customers.Commands.Delete;

public class DeleteCustomerCommand : BaseDeleteCommand, IRequest<bool>
{
    public DeleteCustomerCommand(Guid id) : base(id)
    {
    }
}