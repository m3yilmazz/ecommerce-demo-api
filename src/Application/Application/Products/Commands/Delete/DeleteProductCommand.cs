using Application.Application.Base.Commands;
using MediatR;

namespace Application.Application.Products.Commands.Delete;

public class DeleteProductCommand : BaseDeleteCommand, IRequest<bool>
{
    public DeleteProductCommand(Guid id) : base(id)
    {
    }
}