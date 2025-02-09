using Core.Data;
using Core.Domain.Products;
using Infrastructure.Repository.Base;

namespace Infrastructure.Repository.Products;

public class ProductRepository : EfBaseRepository<Product>, IProductRepository
{
    public ProductRepository(Context context) : base(context)
    {
    }
}