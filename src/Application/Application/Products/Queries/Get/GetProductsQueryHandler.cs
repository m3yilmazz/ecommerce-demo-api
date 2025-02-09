using Application.Application.Models;
using Application.Application.Products.Dtos;
using Application.Application.Products.Mappers;
using Core.Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Application.Products.Queries.Get;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, ArrayBaseResponse<ProductDto>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ArrayBaseResponse<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var queryable = _productRepository.FindAllAsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Name))
            queryable = queryable.Where(w => w.Name.Contains(request.Name));

        var totalData = await queryable.CountAsync(cancellationToken: cancellationToken);

        var result = await queryable
           .Skip(request.PageIndex * request.PageLength)
           .Take(request.PageLength)
           .ToListAsync(cancellationToken: cancellationToken);

        return new ArrayBaseResponse<ProductDto>(
            result.Select(s => s.Map()).ToList(),
            totalData,
            request.PageLength,
            request.PageIndex);
    }
}