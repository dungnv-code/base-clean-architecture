# User Management API — Tài liệu dự án

Clean Architecture (.NET 8) — API quản lý người dùng.

## 1. Cấu trúc dự án

| Layer | Project | Vai trò |
|-------|---------|---------|
| Domain | `Domain` | Entities, không phụ thuộc gì |
| Application | `Application` | DTO, interfaces (IRepository, IService), Service, DependencyInjection |
| Infrastructure | `Infrastructure` | AppDbContext, Migrations, Repository, DependencyInjection |
| Presentation | `Api` | Controllers, Program.cs, appsettings |

Quan hệ tham chiếu:

```
Domain <- Application <- Infrastructure <- Api (Startup Project)
```

DI được đăng ký qua:
- `Application/DependencyInjection.cs` → `AddApplication()`
- `Infrastructure/DependencyInjection.cs` → `AddInfrastructure(configuration)`, được gọi trong `Api/Program.cs`

## 2. Yêu cầu môi trường

- .NET 8 SDK (>= 8.0.400)
- SQL Server 2019+ (bản hiện tại dùng `.\SQLEXPRESS`)
- dotnet-ef tool (xem mục 3.2)

## 3. Cách chạy dự án từ đầu đến hết

### 3.1. Restore package

```bash
dotnet restore
```

### 3.2. Cài tool EF Core (chỉ cần 1 lần)

```bash
dotnet tool install --global dotnet-ef
# nếu đã cài, cập nhật lên bản mới nhất
dotnet tool update --global dotnet-ef
```

✓ Kiểm tra đã cài thành công: `dotnet ef --version`

### 3.3. Cấu hình connection string

Sửa `Api/appsettings.json`:

```json
"DefaultConnection": "Data Source=localhost\\SQLEXPRESS;Initial Catalog=Demo;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;"
```

> ⚠️ `Api/appsettings.Development.json` sẽ **ghi đè** `appsettings.json` khi chạy ở môi trường `Development` (mặc định). Project này dùng **SQL Server** qua `UseSqlServer`, nên **không được** để connection string PostgreSQL (`Host=...`) trong file Development — sẽ lỗi `Keyword not supported: 'host'`.

### 3.4. Tạo database (nếu chưa có bảng nào)

```bash
dotnet ef database update --project Infrastructure --startup-project Api
```

Lệnh này sẽ tạo DB `Demo` và chạy toàn bộ migrations chưa áp dụng.

### 3.5. Chạy API

```bash
dotnet run --project Api
```

hoặc chọn profile **https** trong Visual Studio / VS Code.

- Swagger: `https://localhost:7151/swagger`
- Health check: `https://localhost:7151/health`
- HTTP: `http://localhost:5156`

## 4. Làm việc với Migration

Chu kỳ chuẩn khi thay đổi model:

1. Thay đổi entity trong `Domain/Entities` (hoặc `AppDbContext`).
2. Tạo migration mới:

```bash
dotnet ef migrations add TenMigration --project Infrastructure --startup-project Api
```

3. Áp dụng vào DB:

```bash
dotnet ef database update --project Infrastructure --startup-project Api
```

### 4.1. Lưu ý quan trọng

- **LUÔN LUÔN** kèm `--project Infrastructure --startup-project Api`.
  - `Infrastructure` chứa `AppDbContext` và thư mục `Migrations`.
  - `Api` là startup project có DI đăng ký sẵn DbContext.
- Nếu bỏ `--startup-project`, EF sẽ lấy luôn `Infrastructure` làm host → class library không có DI → lỗi:
  `Unable to resolve service for type 'DbContextOptions<...AppDbContext>'`.
- Migrations chỉ được tạo trong project `Infrastructure` (cấu hình `MigrationsAssembly` trong `Infrastructure/DependencyInjection.cs`).

## 5. Những điều cần lưu ý

### 5.1. Version EF Core phải đồng bộ

Các gói `Microsoft.EntityFrameworkCore`, `.Design`, `.SqlServer` trong **cả `Infrastructure.csproj` và `Api.csproj`** hiện đang dùng **8.0.30**. Khi nâng/hạ thì phải đổi đồng bộ tất cả, nếu không sẽ gặp:

- `NU1605: Detected package downgrade` (Warning as Error làm build fail)
- `MSB3277: version conflict Microsoft.EntityFrameworkCore.Relational`

### 5.2. `migrations add` vs `database update`

- `dotnet ef migrations add` — **CHỈ tạo file**, không chạm DB. Chạy khi thay đổi model.
- `dotnet ef database update` — **áp dụng migration vào DB**. Chạy khi có migration mới.

### 5.3. Xóa / hoàn tác migration

Chưa chạy vào DB — xóa file migration vừa tạo:

```bash
dotnet ef migrations remove --project Infrastructure --startup-project Api
```

Đã chạy vào DB — rollback về một migration cũ:

```bash
dotnet ef database update TenMigrationCu --project Infrastructure --startup-project Api
```

### 5.4. Migration gốc duy nhất

Hiện tại chỉ có 1 migration: `InitialCreate` — tạo bảng `Users` (chứa code thật).

Nếu đã có DB cũ chứa bảng `Cho` từ lần đổi mẫu CRUD, cần dọn lại trước khi áp migration:

```bash
dotnet ef database update 0 --project Infrastructure --startup-project Api
dotnet ef migrations remove --project Infrastructure --startup-project Api
```

### 5.5. Không commit bí mật

Connection string có chứa mật khẩu (như chuỗi PostgreSQL cũ) thì **không** nên commit lên git. Nên dùng `appsettings.Development.json` cho máy local, hoặc biến môi trường / User Secrets:

```bash
dotnet user-secrets init --project Api
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Connection String ..."
```

### 5.6. Bảo mật API

Hiện tại `/health` và Swagger mở cho mọi phương thức qua CORS (`AllowAnyOrigin`). Chưa có xác thực (JWT). Khi thêm bảo mật, cần đóng lại `AllowAnyOrigin`.

## 6. Mở rộng dự án — thêm Entity mới

1. Tạo entity trong `Domain/Entities` (kế thừa `BaseEntity`).
2. Thêm `DbSet` trong `Infrastructure/Persistence/AppDbContext.cs`.
3. (Tùy chọn) Tạo `I...Repository` + `Repository` trong `Application/IRepositories` và `Infrastructure/Repositories`, đăng ký trong `Infrastructure/DependencyInjection.cs`.
4. Tạo service + DTO trong `Application`.
5. Tạo controller trong `Api/Controllers` (kế thừa `BaseController` → route tự động `api/[tên]`).
6. Chạy `migrations add` + `database update` theo mục 4.

## 7. Lệnh thường dùng (cheat sheet)

```bash
dotnet restore
dotnet build
dotnet run --project Api

dotnet ef migrations add TenMigration --project Infrastructure --startup-project Api
dotnet ef database update --project Infrastructure --startup-project Api
dotnet ef migrations list --project Infrastructure --startup-project Api
dotnet ef migrations remove --project Infrastructure --startup-project Api
dotnet ef --version
```