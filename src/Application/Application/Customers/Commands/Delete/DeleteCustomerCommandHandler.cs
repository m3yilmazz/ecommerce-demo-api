using Core.Domain.Base;
using Core.Domain.Customers;
using Core.Domain.Errors.Exceptions;
using MediatR;

namespace Application.Application.Customers.Commands.Delete;

public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, bool>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCustomerCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.FindByIdAsync(request.Id) ??
           throw new NotFoundException($"There is no customer with given {request.Id} ID.");

        _customerRepository.Delete(customer);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}