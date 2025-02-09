using Application.Application.Base.Commands;
using Application.Application.Models;
using Application.Application.Products.Dtos;
using MediatR;

namespace Application.Application.Products.Commands.Update;

public class UpdateProductCommand : BaseUpdateCommand, IRequest<ObjectBaseResponse<ProductDto>>
{
    public string Name { get; }
    public double Price { get; }

    public UpdateProductCommand(Guid id, string name, double price) : base(id)
    {
        Name = name;
        Price = price;
    }
}