using Core.Domain.Base;
using Core.Domain.Customers;
using Core.Domain.Orders;
using Core.Domain.Products;
using Infrastructure.Repository.Customers;
using Infrastructure.Repository.Orders;
using Infrastructure.Repository.Products;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Repository;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}