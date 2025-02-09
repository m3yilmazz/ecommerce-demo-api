using Application.Application.Models;
using Application.Application.Products.Dtos;
using MediatR;

namespace Application.Application.Products.Queries.GetById;

public class GetProductByIdQuery : IRequest<ObjectBaseResponse<ProductDto>>
{
    public Guid Id { get; set; }

    public GetProductByIdQuery(Guid id)
    {
        Id = id;
    }
}