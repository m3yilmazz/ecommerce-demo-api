using Application.Application.Customers.Dtos;
using Application.Application.Customers.Mappers;
using Application.Application.Models;
using Core.Domain.Base;
using Core.Domain.Customers;
using Core.Domain.Errors.Exceptions;
using MediatR;

namespace Application.Application.Customers.Commands.Update;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, ObjectBaseResponse<CustomerDto>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCustomerCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ObjectBaseResponse<CustomerDto>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.FindByIdAsync(request.Id) ??
            throw new NotFoundException($"There is no customer with given {request.Id} ID.");

        var anotherCustomerExist = await _customerRepository.IsExistsAsync(s =>
            s.Id != request.Id &&
            s.Name == request.Name &&
            s.LastName == request.LastName &&
            s.Address == request.Address &&
            s.PostalCode == request.PostalCode);
        if (anotherCustomerExist) throw new ConflictException("Another customer already exist.");

        customer.SetName(request.Name);
        customer.SetLastName(request.LastName);
        customer.SetAddress(request.Address);
        customer.SetPostalCode(request.PostalCode);
        customer.SetUpdatedAt();

        _customerRepository.Update(customer);
        await _unitOfWork.SaveChangesAsync();

        return new ObjectBaseResponse<CustomerDto>(customer.Map());
    }
}