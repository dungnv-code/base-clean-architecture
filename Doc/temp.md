# Kế hoạch triển khai — Gợi ý code

## Phase 1: Fix Login + JWT (ưu tiên cao nhất)

### 1.1. Fix route Login — `UserController.cs`

```csharp
[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] UserLoginDTO request) =>
    Ok(await _service.LoginAsync(request));
```

### 1.2. Hoàn thiện `UserService.LoginAsync` + tạo JWT

```csharp
public async Task<TResponse<UserLoginReponseDTO>> LoginAsync(UserLoginDTO request)
{
    if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        return TResponse<UserLoginReponseDTO>.FailResponse(UserMessage.INVALID_CREDENTIALS, 400);

    var user = await _repo.GetByEmailAsync(request.Email);
    if (user == null || user.IsDeleted)
        return TResponse<UserLoginReponseDTO>.FailResponse(UserMessage.INVALID_CREDENTIALS, 401);

    if (!HashPass.VerifyPassword(user.Password, request.Password))
        return TResponse<UserLoginReponseDTO>.FailResponse(UserMessage.NOT_PASS, 401);

    // claims roles (phase 2: roles + permissions + scope)
    var roles = await _repo.GetRolesByUserIdAsync(user.Id);
    var token = _jwt.GenerateAccessToken(user, roles);

    var refresh = _jwt.GenerateRefreshToken(user.Id);
    await _refreshTokenRepo.CreateAsync(refresh);   // lưu DB

    return TResponse<UserLoginReponseDTO>.SuccessResponse(new UserLoginReponseDTO
    {
        AccessToken = token,
        RefreshToken = refresh.Token,
        IdUser = user.Id
    });
}
```

### 1.3. Service mới `Application/Services/JwtTokenService.cs`

```csharp
public class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _jwt;
    public JwtTokenService(IOptions<JwtSettings> jwt) => _jwt = jwt.Value;

    public string GenerateAccessToken(User user, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new("sub", user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.Name),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        claims.AddRange(roles.Select(r => new Claim("roles", r)));
        // Phase 2: thêm claims "permissions", "school_id", "class_id", "subject_id"

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        return new JwtSecurityToken(
            issuer: _jwt.Issuer, audience: _jwt.Audience,
            claims: claims, expires: DateTime.UtcNow.AddMinutes(_jwt.ExpirationMinutes),
            signingCredentials: creds);
        // .WriteToken(...)
    }

    public RefreshToken GenerateRefreshToken(Guid userId) => new()
    {
        UserId = userId,
        Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
        ExpiresAt = DateTime.UtcNow.AddDays(7),
        CreatedByIp = ""  // inject IHttpContextAccessor hoặc truyền vào
    };
}
```

### 1.4. Wire-up `Api/Program.cs` + `Application/DependencyInjection.cs`

```csharp
// Application/DI
services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
services.AddScoped<IJwtTokenService, JwtTokenService>();

// Api/Program.cs — sau AddInfrastructure
var jwt = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, ValidateAudience = true,
            ValidateLifetime = true, ValidateIssuerSigningKey = true,
            ValidIssuer = jwt!.Issuer, ValidAudience = jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Secret)),
            ClockSkew = TimeSpan.FromMinutes(1)
        };
        options.MapInboundClaims = false;
        options.TokenValidationParameters.RoleClaimType = "roles";
    });
// app.UseAuthentication(); app.UseAuthorization();  (trước MapControllers)
```

`Program.cs` cần truyền `builder.Configuration` vào `AddInfrastructure`/`AddApplication` để bind JwtSettings.

---

## Phase 2: Soft-delete

```csharp
// IRepository + Repository
Task<bool> SoftDeleteAsync(Guid id);
// trong Repository<T>:
public async Task<bool> SoftDeleteAsync(Guid id)
{
    var entity = await _dbSet.FindAsync(id);
    if (entity is not ISoftDelete s || s.IsDeleted) return false;
    s.IsDeleted = true;
    _context.Entry(entity).State = EntityState.Modified;
    await _context.SaveChangesAsync();
    return true;
}
```

```csharp
// BaseEntity hoặc interface riêng
public interface ISoftDelete { bool IsDeleted { get; set; } }
```

- `UserService.DeleteAsync` → gọi `SoftDeleteAsync`
- Global query filter trong `AppDbContext.OnModelCreating`:

```csharp
foreach (var t in modelBuilder.Model.GetEntityTypes())
    if (typeof(ISoftDelete).IsAssignableFrom(t.ClrType))
        modelBuilder.Entity(t.ClrType).HasQueryFilter(...); // reflection
```

---

## Phase 3: RBAC theo KienTruc.md (Phase 1 roadmap)

Thứ tự implement:

| # | Việc | File mới/sửa |
|---|------|--------------|
| 1 | Entities `Permission`, `RolePermission` | `Domain/Entities/` |
| 2 | Fluent configs + unique index + **seed Roles/Permissions** | `Infrastructure/Persistence/Configurations/`, `AppDbContext` |
| 3 | Migration `AddRBACTables` | `dotnet ef migrations add` |
| 4 | `ICurrentUser` + `CurrentUserService` | `Application/` + `Infrastructure/Services/` |
| 5 | `RoleConstants`, `PermissionConstants` | `Application/Constants/` |
| 6 | `PermissionRequirement` + `PermissionHandler` (admin bypass, claim check, DB fallback) | `Application/Authorization/` |
| 7 | `GetRolesByUserIdAsync`, `HasPermissionAsync` | `UserRepository`, `PermissionRepository` |
| 8 | Login load roles+permissions → claims | `JwtTokenService` |
| 9 | `[Authorize(Policy=...)]` trên UserController | `Api` |

```csharp
// Seed Roles (AppDbContext)
modelBuilder.Entity<Role>().HasData(
    new Role { Id = Guid.NewGuid(), Name = "Admin", Code = "admin", ... },
    new Role { ... Code = "school_admin" }, ...);
```

```csharp
// PermissionHandler rút gọn
protected override async Task HandleRequirementAsync(AuthorizationHandlerContext ctx, PermissionRequirement req)
{
    if (_currentUser.IsInRole(RoleConstants.Admin)) { ctx.Succeed(req); return; }
    if (_currentUser.HasPermission(req.Permission)) { ctx.Succeed(req); return; }
    if (await _permRepo.HasPermissionAsync(_currentUser.UserId, req.Permission))
        ctx.Succeed(req);
}
```

---

## Gợi ý thứ tự commit

1. **Commit 1**: JwtTokenService + fix Login route + wire-up Authentication → login chạy được, gọi được endpoint có `[Authorize]`
2. **Commit 2**: Soft-delete + query filter
3. **Commit 3**: RBAC entities + migration + seed
4. **Commit 4**: ICurrentUser + PermissionHandler + policies + `[Authorize]` trên UserController
