using Application.Application.Models;
using Application.Application.Orders.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Application.Orders.Commands.Update;

public class AddOrderItemCommand : IRequest<ObjectBaseResponse<OrderDto>>
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int QuantityOfProduct { get; set; }

    public AddOrderItemCommand(Guid orderId, Guid productId, int quantityOfProduct)
    {
        OrderId = orderId;
        ProductId = productId;
        QuantityOfProduct = quantityOfProduct;
    }
}