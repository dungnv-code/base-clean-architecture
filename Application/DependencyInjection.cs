#nullable disable

using Application.IServices;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IChoService, ChoService>();
        services.AddScoped<IKhuVucService, KhuVucService>();
        services.AddScoped<IKiotService, KiotService>();
        services.AddScoped<IThuongNhanService, ThuongNhanService>();
        services.AddScoped<IHopDongService, HopDongService>();
        services.AddScoped<IHoaDonService, HoaDonService>();
        services.AddScoped<IThanhToanService, ThanhToanService>();
        services.AddScoped<ITaiSanService, TaiSanService>();
        services.AddScoped<ISuCoService, SuCoService>();
        services.AddScoped<INguoiDungService, NguoiDungService>();
        services.AddScoped<ILichKiemTraService, LichKiemTraService>();

        return services;
    }
}
