using Application.Application.Models;
using Application.Application.Products.Dtos;
using MediatR;

namespace Application.Application.Products.Commands.Create;

public class CreateProductCommand : IRequest<ObjectBaseResponse<ProductDto>>
{
    public string Name { get; }
    public double Price { get; }

    private CreateProductCommand()
    {

    }

    public CreateProductCommand(string name, double price)
    {
        Name = name;
        Price = price;
    }
}