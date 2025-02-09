namespace Application.Application.Orders.Dtos;

public class OrderDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public double TotalPrice { get; set; }
    public List<ItemDto> Items { get; set; }

    public OrderDto(Guid id, Guid customerId, DateTime orderDate, double totalPrice, List<ItemDto> items)
    {
        Id = id;
        CustomerId = customerId;
        OrderDate = orderDate;
        TotalPrice = totalPrice;
        Items = items;
    }
}