using Application.Application.Base.Behaviors;
using AspNetCoreRateLimit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterMediatR(this IServiceCollection collection)
    {
        collection.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        return collection;
    }
    
    public static IServiceCollection RegisterLoggerPipelineBehavior(this IServiceCollection collection)
    {
        collection.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));

        return collection;
    }

    public static IServiceCollection RegisterIpRateLimiting(this IServiceCollection collection, IConfiguration configuration)
    {
        collection.AddOptions();
        collection.AddMemoryCache();
        collection.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
        collection.AddInMemoryRateLimiting();
        collection.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

        return collection;
    }
}