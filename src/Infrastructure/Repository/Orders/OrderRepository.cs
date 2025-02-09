using Core.Data;
using Core.Domain.Orders;
using Infrastructure.Repository.Base;

namespace Infrastructure.Repository.Orders;

public class OrderRepository : EfBaseRepository<Order>, IOrderRepository
{
    public OrderRepository(Context context) : base(context)
    {
    }
}