using Application.Application.Customers.Dtos;
using Application.Application.Customers.Mappers;
using Application.Application.Models;
using Core.Domain.Base;
using Core.Domain.Customers;
using Core.Domain.Errors.Exceptions;
using MediatR;

namespace Application.Application.Customers.Commands.Create;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, ObjectBaseResponse<CustomerDto>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCustomerCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ObjectBaseResponse<CustomerDto>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var existRecord = await _customerRepository.FindOneByExpression(s => s.Name == request.Name && s.LastName == s.LastName);

        if (existRecord != null)
            throw new ConflictException($"There is another customer with given {request.Name} name and {request.LastName} lastname.");

        var customer = new Customer(request.Name, request.LastName, request.Address, request.PostalCode);

        await _customerRepository.CreateAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        return new ObjectBaseResponse<CustomerDto>(customer.Map());
    }
}