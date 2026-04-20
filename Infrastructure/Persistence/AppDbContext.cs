using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Cho> Cho { get; set; }
    public DbSet<KhuVuc> KhuVuc { get; set; }
    public DbSet<Kiot> Kiot { get; set; }
    public DbSet<ThuongNhan> ThuongNhan { get; set; }
    public DbSet<HopDong> HopDong { get; set; }
    public DbSet<HoaDon> HoaDon { get; set; }
    public DbSet<ThanhToan> ThanhToan { get; set; }
    public DbSet<TaiSan> TaiSan { get; set; }
    public DbSet<SuCo> SuCo { get; set; }
    public DbSet<NguoiDung> NguoiDung { get; set; }
    public DbSet<LichSuKiot> LichSuKiot { get; set; }
    public DbSet<ChiTietHoaDon> ChiTietHoaDon { get; set; }
    public DbSet<LoaiPhi> LoaiPhi { get; set; }
    public DbSet<LichKiemTra> LichKiemTra { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Cho>().ToTable("Cho");
        modelBuilder.Entity<KhuVuc>().ToTable("KhuVuc");
        modelBuilder.Entity<Kiot>().ToTable("Kiot");
        modelBuilder.Entity<ThuongNhan>().ToTable("ThuongNhan");
        modelBuilder.Entity<HopDong>().ToTable("HopDong");
        modelBuilder.Entity<HoaDon>().ToTable("HoaDon");
        modelBuilder.Entity<ThanhToan>().ToTable("ThanhToan");
        modelBuilder.Entity<TaiSan>().ToTable("TaiSan");
        modelBuilder.Entity<SuCo>().ToTable("SuCo");
        modelBuilder.Entity<NguoiDung>().ToTable("NguoiDung");
        modelBuilder.Entity<LichSuKiot>().ToTable("LichSuKiot");
        modelBuilder.Entity<ChiTietHoaDon>().ToTable("ChiTietHoaDon");
        modelBuilder.Entity<LoaiPhi>().ToTable("LoaiPhi");
        modelBuilder.Entity<LichKiemTra>().ToTable("LichKiemTra");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
