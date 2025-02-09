using Core.Domain.Base;
using Core.Domain.Errors.Exceptions;
using Core.Domain.Orders;
using MediatR;

namespace Application.Application.Orders.Commands.Delete;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.FindByIdAsync(request.Id) ??
            throw new NotFoundException($"There is no order with given {request.Id} ID.");

        _orderRepository.Delete(order);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}