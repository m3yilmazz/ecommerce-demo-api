using Core.Domain.Base;

namespace Core.Domain.Customers;

public interface ICustomerRepository : IEfRepository<Customer>
{
}