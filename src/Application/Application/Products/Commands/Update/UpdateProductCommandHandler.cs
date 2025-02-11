using Application.Application.Models;
using Application.Application.Products.Dtos;
using Application.Application.Products.Mappers;
using Core.Domain.Base;
using Core.Domain.Products;
using Core.Domain.Errors.Exceptions;
using MediatR;

namespace Application.Application.Products.Commands.Update;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ObjectBaseResponse<ProductDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ObjectBaseResponse<ProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.FindByIdAsync(request.Id) ??
            throw new NotFoundException($"There is no product with given {request.Id} ID.");

        var anotherCustomerExist = await _productRepository.IsExistsAsync(s =>
            s.Id != request.Id &&
            s.Name == request.Name);
        if (anotherCustomerExist) throw new ConflictException("Another product with the given name already exist.");

        product.SetName(request.Name);
        product.SetPrice(request.Price);

        _productRepository.Update(product);
        await _unitOfWork.SaveChangesAsync();

        return new ObjectBaseResponse<ProductDto>(product.Map());
    }
}