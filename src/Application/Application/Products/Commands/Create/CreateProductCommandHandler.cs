using Application.Application.Models;
using Application.Application.Products.Dtos;
using Application.Application.Products.Mappers;
using Core.Domain.Base;
using Core.Domain.Errors.Exceptions;
using Core.Domain.Products;
using MediatR;

namespace Application.Application.Products.Commands.Create;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ObjectBaseResponse<ProductDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ObjectBaseResponse<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var existRecord = await _productRepository.FindOneByExpression(s => s.Name == request.Name);

        if (existRecord != null)
            throw new ConflictException($"There is another product with given {request.Name} name.");

        var product = new Product(request.Name, request.Price);

        await _productRepository.CreateAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        return new ObjectBaseResponse<ProductDto>(product.Map());
    }
}