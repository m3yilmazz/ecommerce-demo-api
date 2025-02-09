using Core.Domain.Base;

namespace Core.Domain.Orders;

public interface IOrderRepository : IEfRepository<Order>
{
}