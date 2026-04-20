#nullable disable

using Application.DTOs.Kafka;
using Application.IRepositories;
using Infrastructure.ExternalServices.Kafka;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application;

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

        // Generic repository (dùng cho ChiTietHoaDon và các entity không có typed repo)
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // Typed repositories
        services.AddScoped<IChoRepository, ChoRepository>();
        services.AddScoped<IKhuVucRepository, KhuVucRepository>();
        services.AddScoped<IKiotRepository, KiotRepository>();
        services.AddScoped<IThuongNhanRepository, ThuongNhanRepository>();
        services.AddScoped<IHopDongRepository, HopDongRepository>();
        services.AddScoped<IHoaDonRepository, HoaDonRepository>();
        services.AddScoped<IThanhToanRepository, ThanhToanRepository>();
        services.AddScoped<ITaiSanRepository, TaiSanRepository>();
        services.AddScoped<ISuCoRepository, SuCoRepository>();
        services.AddScoped<INguoiDungRepository, NguoiDungRepository>();
        services.AddScoped<ILichKiemTraRepository, LichKiemTraRepository>();

        // Kafka
        services.AddSingleton<AppSettings>();
        services.AddSingleton<IProducerHandler<AuditLogDTO>, AuditLogProducerHandler>();

        // Application services
        services.AddApplication();

        return services;
    }
}
