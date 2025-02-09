using Application.Application.Models;
using Application.Application.Products.Dtos;
using Application.Application.Products.Mappers;
using Core.Domain.Products;
using Core.Domain.Errors.Exceptions;
using MediatR;

namespace Application.Application.Products.Queries.GetById;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ObjectBaseResponse<ProductDto>>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ObjectBaseResponse<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.FindByIdAsync(request.Id) ??
            throw new NotFoundException($"There is no product with given {request.Id} ID.");

        return new ObjectBaseResponse<ProductDto>(product.Map());
    }
}