using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Data;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PostgresOptions>(configuration.GetSection(nameof(PostgresOptions)));

        var postgresOptions = configuration.GetSection(nameof(PostgresOptions)).Get<PostgresOptions>();

        services.AddDbContextPool<Context>(s => s.UseNpgsql(postgresOptions.ConnectionString));

        return services;
    }
}