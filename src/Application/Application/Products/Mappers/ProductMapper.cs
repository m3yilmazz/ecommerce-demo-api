using Application.Application.Products.Dtos;
using Core.Domain.Products;

namespace Application.Application.Products.Mappers;

public static class ProductMapper
{
    public static ProductDto Map(this Product product) => new(product.Id, product.Name, product.Price);
}