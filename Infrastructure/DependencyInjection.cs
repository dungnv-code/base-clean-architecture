#nullable disable

using Application;
using Application.DTOs.Kafka;
using Infrastructure.ExternalServices.Kafka;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        services.AddSingleton<AppSettings>();
        services.AddSingleton<IProducerHandler<AuditLogDTO>, AuditLogProducerHandler>();

        return services;
    }
}
