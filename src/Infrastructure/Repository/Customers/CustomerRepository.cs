using Core.Data;
using Core.Domain.Customers;
using Infrastructure.Repository.Base;

namespace Infrastructure.Repository.Customers;
public class CustomerRepository : EfBaseRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(Context context) : base(context)
    {
    }
}