using Application.Application.Base.Queries;
using Application.Application.Models;
using Application.Application.Products.Dtos;
using MediatR;

namespace Application.Application.Products.Queries.Get;

public class GetProductsQuery : BaseQuery, IRequest<ArrayBaseResponse<ProductDto>>
{
    public string Name { get; set; }

    public GetProductsQuery()
    {

    }

    public GetProductsQuery(string name, int pageIndex, int pageLength) : base(pageIndex, pageLength)
    {
        Name = name;
    }
}