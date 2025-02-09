using Application.Application.Orders.Dtos;
using Core.Domain.Orders;

namespace Application.Application.Orders.Mappers;

public static class OrderMapper
{
    public static OrderDto Map(this Order order) => new(
        order.Id,
        order.CustomerId,
        order.OrderDate,
        order.TotalPrice,
        order.Items
            .Select(item => new ItemDto(
                item.Id,
                item.ProductId,
                item.Product.Name,
                item.Product.Price,
                item.QuantityOfProduct))
            .ToList());
}