using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewEntitiesAndFKs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Ten",
                table: "ThuongNhan",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "SoDienThoai",
                table: "ThuongNhan",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "GiayPhepKinhDoanh",
                table: "ThuongNhan",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DiaChi",
                table: "ThuongNhan",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "CCCD",
                table: "ThuongNhan",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "TrangThai",
                table: "ThanhToan",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PhuongThuc",
                table: "ThanhToan",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "GhiChu",
                table: "ThanhToan",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "HoaDonId",
                table: "ThanhToan",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaGiaoDich",
                table: "ThanhToan",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ViTri",
                table: "TaiSan",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Ten",
                table: "TaiSan",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Loai",
                table: "TaiSan",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "KhuVucId",
                table: "TaiSan",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "KiotId",
                table: "TaiSan",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayBaoHanh",
                table: "TaiSan",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayMua",
                table: "TaiSan",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QRCode",
                table: "TaiSan",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrangThai",
                table: "TaiSan",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KetQuaXuLy",
                table: "SuCo",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "KiotId",
                table: "SuCo",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayXuLy",
                table: "SuCo",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NguoiBaoCaoId",
                table: "SuCo",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NguoiXuLyId",
                table: "SuCo",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TaiSanId",
                table: "SuCo",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "NguoiDung",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "TrangThai",
                table: "Kiot",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MaKiot",
                table: "Kiot",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LoaiKinhDoanh",
                table: "Kiot",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "KhuVucId",
                table: "Kiot",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ViTri2D",
                table: "Kiot",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ViTri3D",
                table: "Kiot",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ChoId",
                table: "KhuVuc",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GhiChu",
                table: "HopDong",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "KiotId",
                table: "HopDong",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ThuongNhanId",
                table: "HopDong",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TrangThai",
                table: "HoaDon",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "GhiChu",
                table: "HoaDon",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "HopDongId",
                table: "HoaDon",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaHoaDon",
                table: "HoaDon",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayDaoHan",
                table: "HoaDon",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Ten",
                table: "Cho",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "DiaChi",
                table: "Cho",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.CreateTable(
                name: "ChiTietHoaDon",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HoaDonId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LoaiPhiId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TenKhoanPhi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SoLuong = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ThanhTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietHoaDon", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LichKiemTra",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KhuVucId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NguoiPhuTrachId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TieuDe = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LoaiKiemTra = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NgayKiemTra = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    KetQua = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichKiemTra", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LichSuKiot",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KiotId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ThuongNhanCuId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ThuongNhanMoiId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HopDongCuId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HopDongMoiId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LoaiBienDong = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NgayBanGiao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LyDo = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichSuKiot", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoaiPhi",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenPhi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MaPhi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DonViTinh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GiaMacDinh = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiPhi", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietHoaDon");

            migrationBuilder.DropTable(
                name: "LichKiemTra");

            migrationBuilder.DropTable(
                name: "LichSuKiot");

            migrationBuilder.DropTable(
                name: "LoaiPhi");

            migrationBuilder.DropColumn(
                name: "GhiChu",
                table: "ThanhToan");

            migrationBuilder.DropColumn(
                name: "HoaDonId",
                table: "ThanhToan");

            migrationBuilder.DropColumn(
                name: "MaGiaoDich",
                table: "ThanhToan");

            migrationBuilder.DropColumn(
                name: "KhuVucId",
                table: "TaiSan");

            migrationBuilder.DropColumn(
                name: "KiotId",
                table: "TaiSan");

            migrationBuilder.DropColumn(
                name: "NgayBaoHanh",
                table: "TaiSan");

            migrationBuilder.DropColumn(
                name: "NgayMua",
                table: "TaiSan");

            migrationBuilder.DropColumn(
                name: "QRCode",
                table: "TaiSan");

            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "TaiSan");

            migrationBuilder.DropColumn(
                name: "KetQuaXuLy",
                table: "SuCo");

            migrationBuilder.DropColumn(
                name: "KiotId",
                table: "SuCo");

            migrationBuilder.DropColumn(
                name: "NgayXuLy",
                table: "SuCo");

            migrationBuilder.DropColumn(
                name: "NguoiBaoCaoId",
                table: "SuCo");

            migrationBuilder.DropColumn(
                name: "NguoiXuLyId",
                table: "SuCo");

            migrationBuilder.DropColumn(
                name: "TaiSanId",
                table: "SuCo");

            migrationBuilder.DropColumn(
                name: "KhuVucId",
                table: "Kiot");

            migrationBuilder.DropColumn(
                name: "ViTri2D",
                table: "Kiot");

            migrationBuilder.DropColumn(
                name: "ViTri3D",
                table: "Kiot");

            migrationBuilder.DropColumn(
                name: "ChoId",
                table: "KhuVuc");

            migrationBuilder.DropColumn(
                name: "GhiChu",
                table: "HopDong");

            migrationBuilder.DropColumn(
                name: "KiotId",
                table: "HopDong");

            migrationBuilder.DropColumn(
                name: "ThuongNhanId",
                table: "HopDong");

            migrationBuilder.DropColumn(
                name: "GhiChu",
                table: "HoaDon");

            migrationBuilder.DropColumn(
                name: "HopDongId",
                table: "HoaDon");

            migrationBuilder.DropColumn(
                name: "MaHoaDon",
                table: "HoaDon");

            migrationBuilder.DropColumn(
                name: "NgayDaoHan",
                table: "HoaDon");

            migrationBuilder.AlterColumn<string>(
                name: "Ten",
                table: "ThuongNhan",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SoDienThoai",
                table: "ThuongNhan",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "GiayPhepKinhDoanh",
                table: "ThuongNhan",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DiaChi",
                table: "ThuongNhan",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CCCD",
                table: "ThuongNhan",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TrangThai",
                table: "ThanhToan",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "PhuongThuc",
                table: "ThanhToan",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "ViTri",
                table: "TaiSan",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Ten",
                table: "TaiSan",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Loai",
                table: "TaiSan",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "NguoiDung",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TrangThai",
                table: "Kiot",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "MaKiot",
                table: "Kiot",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "LoaiKinhDoanh",
                table: "Kiot",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TrangThai",
                table: "HoaDon",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Ten",
                table: "Cho",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DiaChi",
                table: "Cho",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
