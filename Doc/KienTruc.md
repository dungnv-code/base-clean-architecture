# Kiến Trúc Phân Quyền RBAC — School Management API

> Tài liệu thiết kế & implement chi tiết hệ thống **Role-Based Access Control (RBAC)** cho hệ thống quản lý trường học (.NET 8, Clean Architecture).

---

## 1. Tổng Quan

### 1.1. Mục tiêu

- **6 vai trò** phân quyền chặt chẽ theo phạm vi dữ liệu (scope).
- Hỗ trợ **RBAC đầy đủ**: Role → Permission (Resource + Action), thay đổi quyền linh hoạt không cần sửa code.
- Kết hợp **scope-based authorization**: mỗi user chỉ thao tác dữ liệu thuộc phạm vi của mình (trường, bộ môn, lớp, cá nhân).

### 1.2. Vai trò & Phạm vi

| Role | Mã | Phạm vi (Scope) | Mô tả |
|------|----|-----------------|--------|
| **Admin** | `admin` | Global (toàn hệ thống) | Quản trị hệ thống, schools, users, roles, cấu hình |
| **SchoolAdmin** | `school_admin` | School (`SchoolId`) | Quản lý 1 trường: teachers, students, subjects, schedules |
| **SubjectAdmin** | `subject_admin` | School + Subject (`SchoolId`, `SubjectId`) | Quản lý bộ môn: điểm, bài tập, giáo trình |
| **Teacher** | `teacher` | School + Class/Subject (`SchoolId`, `ClassId`, `SubjectId`) | Dạy lớp, chấm điểm, điểm danh |
| **Student** | `student` | School + Class + Self (`SchoolId`, `ClassId`, `UserId`) | Xem điểm, lịch, bài tập của mình |
| **Guest** | `guest` | Public (read-only) | Xem thông tin công khai, không cần đăng nhập |

### 1.3. Nguyên Tắc Thiết Kế

1. **Permission-based**: Mọi thao tác được định nghĩa bằng `Permission` (Resource + Action), không hardcode role trong controller.
2. **Server-side check**: Policy + Handler xác thực tại tầng Authorization, không tin dữ liệu client gửi lên.
3. **Default Deny**: Không được cấp quyền → denied. Quyền được grant tường minh.
4. **Scope Claims**: JWT chứa claims về phạm vi (`SchoolId`, `ClassId`, ...) để filter/authorize dữ liệu.
5. **Soft delete & Audit**: `IsDeleted`, `CreatedAt`, `UpdatedAt` theo `BaseEntity` cho mọi bảng liên quan quyền.

---

## 2. Kiến Trúc Layered

```
┌─────────────────────────────────────────────────────────────┐
│                          Api Layer                          │
│  Controllers  │  Middleware (Exception/Auth)  │  Extensions │
│  [Authorize(Policy=...)]  │  CurrentUserService            │
└───────────────────────────┬─────────────────────────────────┘
                            │
┌───────────────────────────▼─────────────────────────────────┐
│                     Application Layer                       │
│  DTOs (Role, Permission, User)  │  Services (RoleService,    │
│  IRepositories  │  IAuthorization (ICurrentUser)             │
│  Authorization Handlers (PermissionRequirement/Handler)      │
└───────────────────────────┬─────────────────────────────────┘
                            │
┌───────────────────────────▼─────────────────────────────────┐
│                     Infrastructure Layer                    │
│  AppDbContext (DbSets, Configurations)                       │
│  Repositories (RoleRepository, PermissionRepository...)      │
│  Migrations                                                 │
└───────────────────────────┬─────────────────────────────────┘
                            │
┌───────────────────────────▼─────────────────────────────────┐
│                        Domain Layer                         │
│  Entities: User, Roles, UserRoles, Permissions,              │
│            RolePermissions, Schools, Classes, Subjects...    │
│  Enums: UserRole (legacy), Resource, Action (enum)           │
│  Base: BaseEntity                                            │
└─────────────────────────────────────────────────────────────┘
```

**Dependency Rule**: `Domain ← Application ← Infrastructure ← Api`. Authorization logic nằm ở **Application** (handlers, requirements), wiring ở **Api** (Extensions).

---

## 3. Database Schema (Entity Framework Core)

### 3.1. Bảng chính

```sql
-- Users (đã có)
Users (Id, Name, Email, PhoneNumber, Password, Address, Avatar, IsDeleted, CreatedAt, UpdatedAt)

-- Roles (đã có, cần seed)
Roles (Id, Name, Code, IsDeleted, CreatedAt, UpdatedAt)
-- Seed: Admin, SchoolAdmin, SubjectAdmin, Teacher, Student, Guest

-- UserRoles (đã có, join)
UserRoles (Id, UserId, RoleId, IsDeleted, CreatedAt, UpdatedAt)

-- Permissions (MỚI)
Permissions (Id, Resource, Action, Name, Description, IsDeleted, CreatedAt, UpdatedAt)
-- Unique (Resource, Action)

-- RolePermissions (MỚI, join)
RolePermissions (Id, RoleId, PermissionId, IsDeleted, CreatedAt, UpdatedAt)

-- Scope claims tạm lưu trên User (hoặc bảng riêng UserScopes nếu nhiều trường)
Users.SchoolId (nullable Guid)        -- SchoolAdmin, Teacher, Student
Users.SubjectId (nullable Guid)       -- SubjectAdmin, Teacher (bộ môn chính)
Users.ClassId (nullable Guid)         -- Teacher (lớp chủ nhiệm), Student
```

### 3.2. Entities (Domain Layer)

```csharp
// Domain/Entities/Roles.cs (đã có, mở rộng)
public class Roles : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty; // "admin", "school_admin", ...
    public ICollection<UserRoles> Users { get; set; } = new List<UserRoles>();
    public ICollection<RolePermissions> Permissions { get; set; } = new List<RolePermissions>();
}

// Domain/Entities/UserRoles.cs (đã có)
public class UserRoles : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid RoleId { get; set; }
    public Roles Role { get; set; } = null!;
}

// Domain/Entities/Permissions.cs (MỚI)
public class Permissions : BaseEntity
{
    public string Resource { get; set; } = string.Empty; // "User", "School", "Subject", "Class", "Score", "Assignment", "Schedule", "News"
    public string Action { get; set; } = string.Empty;    // "Create", "Read", "Update", "Delete", "Approve", "Export"
    public string Name { get; set; } = string.Empty;      // "school.create"
    public string? Description { get; set; }
    public ICollection<RolePermissions> Roles { get; set; } = new List<RolePermissions>();
}

// Domain/Entities/RolePermissions.cs (MỚI)
public class RolePermissions : BaseEntity
{
    public Guid RoleId { get; set; }
    public Roles Role { get; set; } = null!;
    public Guid PermissionId { get; set; }
    public Permissions Permission { get; set; } = null!;
}
```

### 3.3. Seed Data

**Roles:**

| Code | Name |
|------|------|
| `admin` | Admin |
| `school_admin` | School Admin |
| `subject_admin` | Subject Admin |
| `teacher` | Teacher |
| `student` | Student |
| `guest` | Guest |

**Permissions (ví dụ):**

| Resource | Actions |
|----------|---------|
| `User` | Create, Read, Update, Delete, AssignRole |
| `School` | Create, Read, Update, Delete |
| `Subject` | Create, Read, Update, Delete, AssignTeacher |
| `Class` | Create, Read, Update, Delete, Enroll |
| `Score` | Create, Read, Update, Delete, Approve, Export |
| `Assignment` | Create, Read, Update, Delete, Submit |
| `Schedule` | Create, Read, Update, Delete |
| `News` | Create, Read, Update, Delete |

**RolePermissions mapping (khuyến nghị):**

| Permission | Admin | SchoolAdmin | SubjectAdmin | Teacher | Student | Guest |
|------------|:-----:|:-----------:|:------------:|:-------:|:-------:|:-----:|
| School.* | ✔ | (scope自己的) | | | | Read only |
| User.Create/Delete | ✔ | ✔ (trong school) | | | | |
| User.Read | ✔ | ✔ (school) | ✔ (school) | ✔ (class) | ✔ (self) | |
| Subject.* | ✔ | ✔ (school) | ✔ (assigned) | Read | Read | Read |
| Class.* | ✔ | ✔ (school) | | ✔ (assigned) | Read (class mình) | |
| Score.Create/Update | ✔ | ✔ | ✔ (môn) | ✔ (môn/mình dạy) | | |
| Score.Read | ✔ | ✔ | ✔ | ✔ (class/môn) | ✔ (self) | |
| Score.Approve | ✔ | ✔ | ✔ | | | |
| Score.Export | ✔ | ✔ | ✔ | ✔ | | |
| Assignment.Create | ✔ | ✔ | ✔ | ✔ | | |
| Assignment.Submit | | | | | ✔ | |
| Schedule.* | ✔ | ✔ | ✔ | Read | Read | Read |
| News.Create/Delete | ✔ | ✔ | | | | |
| News.Read | ✔ | ✔ | ✔ | ✔ | ✔ | ✔ |

### 3.4. AppDbContext (Infrastructure)

```csharp
// Infrastructure/Persistence/AppDbContext.cs
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Roles> Roles => Set<Roles>();
    public DbSet<UserRoles> UserRoles => Set<UserRoles>();
    public DbSet<Permissions> Permissions => Set<Permissions>();          // MỚI
    public DbSet<RolePermissions> RolePermissions => Set<RolePermissions>(); // MỚI

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Cấu hình unique index cho Roles.Code
        modelBuilder.Entity<Roles>()
            .HasIndex(r => r.Code)
            .IsUnique();

        // Cấu hình unique index cho Permissions (Resource + Action)
        modelBuilder.Entity<Permissions>()
            .HasIndex(p => new { p.Resource, p.Action })
            .IsUnique();

        // Cấu hình unique constraint: 1 user chỉ có 1 role (nếu business rule cho phép multi-role thì bỏ)
        modelBuilder.Entity<UserRoles>()
            .HasIndex(ur => new { ur.UserId, ur.RoleId })
            .IsUnique();

        // Cấu hình unique constraint: 1 role chỉ có 1 permission
        modelBuilder.Entity<RolePermissions>()
            .HasIndex(rp => new { rp.RoleId, rp.PermissionId })
            .IsUnique();

        // Seed data
        SeedRoles(modelBuilder);
        SeedPermissions(modelBuilder);
        SeedRolePermissions(modelBuilder);
    }

    private static void SeedRoles(ModelBuilder modelBuilder) { /* ... */ }
    private static void SeedPermissions(ModelBuilder modelBuilder) { /* ... */ }
    private static void SeedRolePermissions(ModelBuilder modelBuilder) { /* ... */ }
}
```

**Migration:**

```bash
dotnet ef migrations add AddRBACTables --project Infrastructure --startup-project Api
dotnet ef database update --project Infrastructure --startup-project Api
```

---

## 4. JWT Claims Structure

### 4.1. Claims khi Login

```json
{
  "sub": "user-guid",
  "email": "user@example.com",
  "name": "Nguyễn Văn A",
  "roles": ["school_admin"],
  "permissions": ["school.read", "user.create", "score.approve"],
  "school_id": "school-guid",
  "class_id": null,
  "subject_id": null,
  "jti": "token-guid",
  "iat": 1234567890,
  "exp": 1234567890
}
```

### 4.2. Claims Generation (Application Layer)

```csharp
// Application/Services/AuthService.cs (mở rộng từ UserService.LoginAsync)
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public async Task<UserLoginResponseDTO> LoginAsync(UserLoginDTO dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null || !VerifyPassword(dto.Password, user.Password))
            throw new UnauthorizedAccessException("Invalid credentials");

        // Lấy roles + permissions từ DB
        var roles = await _roleRepository.GetRolesByUserIdAsync(user.Id);
        var permissions = await _roleRepository.GetPermissionsByRolesAsync(roles.Select(r => r.Id));

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.Name),
        };

        // Roles claim
        foreach (var role in roles)
            claims.Add(new Claim("roles", role.Code));

        // Permissions claim (nếu không quá nhiều, nếu nhiều thì chỉ cache role và query DB)
        foreach (var perm in permissions)
            claims.Add(new Claim("permissions", perm.Name));

        // Scope claims
        if (user.SchoolId.HasValue)
            claims.Add(new Claim("school_id", user.SchoolId.Value.ToString()));
        if (user.ClassId.HasValue)
            claims.Add(new Claim("class_id", user.ClassId.Value.ToString()));
        if (user.SubjectId.HasValue)
            claims.Add(new Claim("subject_id", user.SubjectId.Value.ToString()));

        var token = GenerateJwtToken(claims);
        return new UserLoginResponseDTO { AccessToken = token, IdUser = user.Id };
    }
}
```

---

## 5. Authorization Flow

### 5.1. Luồng xác thực & phân quyền

```
Client Request
    │
    ├── Header: Authorization: Bearer <jwt>
    │
    ▼
┌─────────────────────┐
│ JwtBearerMiddleware │  ← Validate token, build ClaimsPrincipal
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│  [Authorize] attr   │  ← Check Role policy (coarse-grained)
│  on Controller/     │
│  Action             │
└──────────┬──────────┘
           │ (pass)
           ▼
┌─────────────────────────────┐
│ PermissionHandler (custom)  │  ← Check Permission claim / DB (fine-grained)
│  IAuthorizationRequirement  │
│  PermissionRequirement      │
└──────────┬──────────────────┘
           │ (pass)
           ▼
┌─────────────────────────────┐
│ Controller Action           │
│  ├── Inject ICurrentUser    │  ← Lấy userId, schoolId, roles từ Claims
│  ├── Service Layer          │  ← Business logic + scope filter
│  └── Repository/EF Query    │  ← WHERE SchoolId = currentSchoolId ...
└─────────────────────────────┘
```

### 5.2. Các tầng check

| Tầng | Cơ chế | Ví dụ |
|------|--------|-------|
| **Endpoint** | `[Authorize(Policy = "ScoreEdit")]` | Action yêu cầu permission `score.update` |
| **Resource** | `IAuthorizationRequirement` + Handler với resource params | Check user có permission VÀ thuộc đúng school/class |
| **Data** | Repository query filter theo scope claim | `WHERE SchoolId = @currentSchoolId` |

---

## 6. Implementation Code

### 6.1. Application Layer

#### 6.1.1. Constants

```csharp
// Application/Constants/PermissionConstants.cs
public static class PermissionConstants
{
    // Resources
    public const string ResourceUser = "User";
    public const string ResourceSchool = "School";
    public const string ResourceSubject = "Subject";
    public const string ResourceClass = "Class";
    public const string ResourceScore = "Score";
    public const string ResourceAssignment = "Assignment";
    public const string ResourceSchedule = "Schedule";
    public const string ResourceNews = "News";

    // Actions
    public const string ActionCreate = "Create";
    public const string ActionRead = "Read";
    public const string ActionUpdate = "Update";
    public const string ActionDelete = "Delete";
    public const string ActionApprove = "Approve";
    public const string ActionExport = "Export";
    public const string ActionAssign = "Assign";
    public const string ActionSubmit = "Submit";

    // Policy Names
    public static class Policies
    {
        public const string UserManage = "UserManage";
        public const string SchoolManage = "SchoolManage";
        public const string SubjectManage = "SubjectManage";
        public const string ClassManage = "ClassManage";
        public const string ScoreManage = "ScoreManage";
        public const string ScoreApprove = "ScoreApprove";
        public const string ScoreExport = "ScoreExport";
        public const string AssignmentManage = "AssignmentManage";
        public const string AssignmentSubmit = "AssignmentSubmit";
        public const string ScheduleManage = "ScheduleManage";
        public const string NewsManage = "NewsManage";
        public const string NewsRead = "NewsRead";
    }
}

// Application/Constants/RoleConstants.cs
public static class RoleConstants
{
    public const string Admin = "admin";
    public const string SchoolAdmin = "school_admin";
    public const string SubjectAdmin = "subject_admin";
    public const string Teacher = "teacher";
    public const string Student = "student";
    public const string Guest = "guest";
}
```

#### 6.1.2. Authorization Requirements & Handlers

```csharp
// Application/Authorization/Requirements/PermissionRequirement.cs
public class PermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; }
    public PermissionRequirement(string permission) => Permission = permission;
}

// Application/Authorization/Requirements/ScopeRequirement.cs
public class ScopeRequirement : IAuthorizationRequirement
{
    public string ScopeType { get; } // "School", "Class", "Subject"
    public ScopeRequirement(string scopeType) => ScopeType = scopeType;
}

// Application/Authorization/Handlers/PermissionHandler.cs
public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly ICurrentUser _currentUser;
    private readonly IPermissionRepository _permissionRepository;

    public PermissionHandler(ICurrentUser currentUser, IPermissionRepository permissionRepository)
    {
        _currentUser = currentUser;
        _permissionRepository = permissionRepository;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        // Admin luôn pass
        if (_currentUser.IsInRole(RoleConstants.Admin))
        {
            context.Succeed(requirement);
            return;
        }

        // Check permission claim trong JWT
        if (_currentUser.HasPermission(requirement.Permission))
        {
            context.Succeed(requirement);
            return;
        }

        // Fallback: query DB (nếu claim không load đủ)
        var userId = _currentUser.UserId;
        var hasPermission = await _permissionRepository
            .HasPermissionAsync(userId, requirement.Permission);

        if (hasPermission)
            context.Succeed(requirement);
    }
}

// Application/Authorization/Handlers/ScopeHandler.cs
public class ScopeHandler : AuthorizationHandler<ScopeRequirement>
{
    private readonly ICurrentUser _currentUser;

    public ScopeHandler(ICurrentUser currentUser) => _currentUser = currentUser;

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ScopeRequirement requirement)
    {
        switch (requirement.ScopeType)
        {
            case "School":
                if (_currentUser.SchoolId.HasValue)
                    context.Succeed(requirement);
                break;
            case "Class":
                if (_currentUser.ClassId.HasValue)
                    context.Succeed(requirement);
                break;
            case "Subject":
                if (_currentUser.SubjectId.HasValue)
                    context.Succeed(requirement);
                break;
        }
        return Task.CompletedTask;
    }
}
```

#### 6.1.3. ICurrentUser & CurrentUserService

```csharp
// Application/ICurrentUser.cs
public interface ICurrentUser
{
    Guid UserId { get; }
    string Email { get; }
    Guid? SchoolId { get; }
    Guid? ClassId { get; }
    Guid? SubjectId { get; }
    bool IsInRole(string roleCode);
    bool HasPermission(string permission);
    IEnumerable<string> GetRoles();
}

// Infrastructure/Services/CurrentUserService.cs
public class CurrentUserService : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId
    {
        get
        {
            var sub = _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value
                      ?? _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(sub ?? Guid.Empty.ToString());
        }
    }

    public string Email =>
        _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;

    public Guid? SchoolId
    {
        get
        {
            var val = _httpContextAccessor.HttpContext?.User?.FindFirst("school_id")?.Value;
            return val != null ? Guid.Parse(val) : null;
        }
    }

    public Guid? ClassId
    {
        get
        {
            var val = _httpContextAccessor.HttpContext?.User?.FindFirst("class_id")?.Value;
            return val != null ? Guid.Parse(val) : null;
        }
    }

    public Guid? SubjectId
    {
        get
        {
            var val = _httpContextAccessor.HttpContext?.User?.FindFirst("subject_id")?.Value;
            return val != null ? Guid.Parse(val) : null;
        }
    }

    public bool IsInRole(string roleCode) =>
        _httpContextAccessor.HttpContext?.User?.IsInRole(roleCode) == true;

    public bool HasPermission(string permission) =>
        _httpContextAccessor.HttpContext?.User?.FindAll("permissions")
            .Any(c => c.Value == permission);

    public IEnumerable<string> GetRoles() =>
        _httpContextAccessor.HttpContext?.User?.FindAll("roles").Select(c => c.Value) ?? Enumerable.Empty<string>();
}
```

#### 6.1.4. Repositories

```csharp
// Application/IRepositories/IPermissionRepository.cs
public interface IPermissionRepository
{
    Task<bool> HasPermissionAsync(Guid userId, string permissionName);
    Task<List<Permissions>> GetPermissionsByRoleIdAsync(Guid roleId);
    Task<List<Permissions>> GetAllPermissionsAsync();
}

// Application/IRepositories/IRoleRepository.cs
public interface IRoleRepository
{
    Task<Roles?> GetByCodeAsync(string code);
    Task<List<Roles>> GetRolesByUserIdAsync(Guid userId);
    Task<List<Permissions>> GetPermissionsByRolesAsync(IEnumerable<Guid> roleIds);
    Task<bool> AssignRoleToUserAsync(Guid userId, Guid roleId);
    Task<bool> GrantPermissionAsync(Guid roleId, Guid permissionId);
}
```

```csharp
// Infrastructure/Repositories/PermissionRepository.cs
public class PermissionRepository : BaseRepository<Permissions>, IPermissionRepository
{
    private readonly AppDbContext _context;

    public PermissionRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> HasPermissionAsync(Guid userId, string permissionName)
    {
        return await _context.RolePermissions
            .AnyAsync(rp =>
                rp.Role.Users.Any(ur => ur.UserId == userId) &&
                rp.Permission.Name == permissionName &&
                !rp.IsDeleted &&
                !rp.Permission.IsDeleted &&
                !rp.Role.IsDeleted);
    }

    public async Task<List<Permissions>> GetPermissionsByRoleIdAsync(Guid roleId)
    {
        return await _context.RolePermissions
            .Where(rp => rp.RoleId == roleId && !rp.IsDeleted)
            .Select(rp => rp.Permission)
            .Where(p => !p.IsDeleted)
            .ToListAsync();
    }

    public async Task<List<Permissions>> GetAllPermissionsAsync()
    {
        return await _context.Permissions
            .Where(p => !p.IsDeleted)
            .ToListAsync();
    }
}
```

### 6.2. Api Layer — Extensions & Middleware

#### 6.2.1. JwtAuthentication Extension

```csharp
// Api/Extensions/AuthenticationExtensions.cs
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
    {
        var secret = config["Jwt:Secret"] ?? throw new InvalidOperationException("Jwt:Secret not configured");
        var issuer = config["Jwt:Issuer"] ?? "SchoolManagementAPI";
        var audience = config["Jwt:Audience"] ?? "SchoolManagementClient";

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                    ClockSkew = TimeSpan.FromMinutes(1)
                };

                // Map role claims
                options.MapInboundClaims = false;
                options.TokenValidationParameters.RoleClaimType = "roles";
                options.TokenValidationParameters.NameClaimType = "name";
            });

        return services;
    }

    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            // Coarse-grained: Role-based policies
            options.AddPolicy(Policies.AdminOnly, p => p.RequireRole(RoleConstants.Admin));
            options.AddPolicy(Policies.SchoolAdminOnly, p => p.RequireRole(RoleConstants.Admin, RoleConstants.SchoolAdmin));
            options.AddPolicy(Policies.TeacherOnly, p => p.RequireRole(RoleConstants.Teacher));
            options.AddPolicy(Policies.StudentOnly, p => p.RequireRole(RoleConstants.Student));

            // Fine-grained: Permission-based policies
            options.AddPolicy(Policies.UserManage, p =>
                p.AddRequirements(new PermissionRequirement("user.manage")));

            options.AddPolicy(Policies.SchoolManage, p =>
                p.AddRequirements(new PermissionRequirement("school.manage")));

            options.AddPolicy(Policies.SubjectManage, p =>
                p.AddRequirements(new PermissionRequirement("subject.manage")));

            options.AddPolicy(Policies.ClassManage, p =>
                p.AddRequirements(new PermissionRequirement("class.manage")));

            options.AddPolicy(Policies.ScoreManage, p =>
                p.AddRequirements(new PermissionRequirement("score.manage")));

            options.AddPolicy(Policies.ScoreApprove, p =>
                p.AddRequirements(new PermissionRequirement("score.approve")));

            options.AddPolicy(Policies.ScoreExport, p =>
                p.AddRequirements(new PermissionRequirement("score.export")));

            options.AddPolicy(Policies.AssignmentManage, p =>
                p.AddRequirements(new PermissionRequirement("assignment.manage")));

            options.AddPolicy(Policies.AssignmentSubmit, p =>
                p.AddRequirements(new PermissionRequirement("assignment.submit")));

            options.AddPolicy(Policies.ScheduleManage, p =>
                p.AddRequirements(new PermissionRequirement("schedule.manage")));

            options.AddPolicy(Policies.NewsManage, p =>
                p.AddRequirements(new PermissionRequirement("news.manage")));

            options.AddPolicy(Policies.NewsRead, p =>
                p.RequireAuthenticatedUser()); // hoặc AllowAnonymous

            // Composite policies: Permission + Scope
            options.AddPolicy("ScoreManageInSchool", p =>
                p.AddRequirements(
                    new PermissionRequirement("score.manage"),
                    new ScopeRequirement("School")));

            options.AddPolicy("ClassManageInSchool", p =>
                p.AddRequirements(
                    new PermissionRequirement("class.manage"),
                    new ScopeRequirement("School")));
        });

        return services;
    }

    public static void AddAuthorizationHandlers(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
        services.AddSingleton<IAuthorizationHandler, ScopeHandler>();
        services.AddScoped<ICurrentUser, CurrentUserService>();
    }
}
```

#### 6.2.2. appsettings.json

```json
{
  "Jwt": {
    "Secret": "your-256-bit-secret-key-here-minimum-32-chars!!",
    "Issuer": "SchoolManagementAPI",
    "Audience": "SchoolManagementClient",
    "ExpirationMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

#### 6.2.3. Program.cs (Api)

```csharp
// Api/Program.cs
var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Application
builder.Services.AddApplication();

// Authentication & Authorization
builder.Services.AddHttpContextAccessor();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorizationPolicies();
builder.Services.AddAuthorizationHandlers();

// CORS
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();   // ← QUAN TRỌNG: phải trước UseAuthorization
app.UseAuthorization();
app.MapControllers();
app.Run();
```

### 6.3. Infrastructure Layer — Dependency Injection

```csharp
// Infrastructure/DependencyInjection.cs
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();         // MỚI
        services.AddScoped<IPermissionRepository, PermissionRepository>(); // MỚI

        return services;
    }
}
```

---

## 7. Controller Examples

### 7.1. UserController (đã có → mở rộng)

```csharp
// Api/Controllers/UserController.cs
[ApiController]
[Route("api/[controller]")]
public class UserController : BaseController
{
    private readonly IUserService _userService;
    private readonly ICurrentUser _currentUser;

    public UserController(IUserService userService, ICurrentUser currentUser)
    {
        _userService = userService;
        _currentUser = currentUser;
    }

    // GET: api/user
    [HttpGet]
    [Authorize(Policy = "UserManage")]  // Chỉ Admin hoặc SchoolAdmin có permission user.manage
    public async Task<IActionResult> GetAll([FromQuery] UserQueryDTO query)
    {
        // Scope filter: SchoolAdmin chỉ thấy users trong trường mình
        if (_currentUser.IsInRole(RoleConstants.SchoolAdmin))
        {
            query.SchoolId = _currentUser.SchoolId;  // ép filter theo school của admin
        }
        var result = await _userService.GetAllAsync(query);
        return Ok(result);
    }

    // GET: api/user/{id}
    [HttpGet("{id:guid}")]
    [Authorize(Policy = "UserRead")]  // Admin, SchoolAdmin (school), SubjectAdmin (school), Teacher (class), Student (self)
    public async Task<IActionResult> GetById(Guid id)
    {
        // Scope check trong service: user chỉ được xem user trong scope của mình
        var result = await _userService.GetByIdAsync(id, _currentUser);
        return result != null ? Ok(result) : NotFound();
    }

    // POST: api/user
    [HttpPost]
    [Authorize(Policy = "UserCreate")]
    public async Task<IActionResult> Create([FromBody] UserRequestDTO dto)
    {
        // SchoolAdmin tạo user → ép SchoolId = school của mình
        if (_currentUser.IsInRole(RoleConstants.SchoolAdmin))
        {
            dto.SchoolId = _currentUser.SchoolId;
        }
        var result = await _userService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // DELETE: api/user/{id}
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "UserDelete")]  // Chỉ Admin
    public async Task<IActionResult> Delete(Guid id)
    {
        await _userService.SoftDeleteAsync(id);
        return NoContent();
    }
}
```

### 7.2. ScoreController (MỚI — ví dụ phân quyền chi tiết)

```csharp
// Api/Controllers/ScoreController.cs
[ApiController]
[Route("api/[controller]")]
[Authorize] // Yêu cầu đăng nhập
public class ScoreController : BaseController
{
    private readonly IScoreService _scoreService;
    private readonly ICurrentUser _currentUser;

    public ScoreController(IScoreService scoreService, ICurrentUser currentUser)
    {
        _scoreService = scoreService;
        _currentUser = currentUser;
    }

    // GET: api/score/student/{studentId}
    [HttpGet("student/{studentId:guid}")]
    [Authorize(Policy = "ScoreRead")]
    public async Task<IActionResult> GetScoresByStudent(Guid studentId)
    {
        // Student chỉ xem điểm của chính mình
        if (_currentUser.IsInRole(RoleConstants.Student) && _currentUser.UserId != studentId)
            return Forbid();

        var scores = await _scoreService.GetScoresByStudentAsync(studentId, _currentUser);
        return Ok(scores);
    }

    // POST: api/score
    [HttpPost]
    [Authorize(Policy = "ScoreManageInSchool")]  // Permission score.manage + Scope School
    public async Task<IActionResult> Create([FromBody] ScoreCreateDTO dto)
    {
        // Service layer check: Teacher chỉ tạo điểm cho môn/lớp mình dạy
        // SubjectAdmin chỉ tạo điểm cho môn mình quản lý
        await _scoreService.CreateScoreAsync(dto, _currentUser);
        return Created();
    }

    // PUT: api/score/{id}/approve
    [HttpPut("{id:guid}/approve")]
    [Authorize(Policy = "ScoreApprove")]  // Chỉ Admin, SchoolAdmin, SubjectAdmin
    public async Task<IActionResult> Approve(Guid id)
    {
        await _scoreService.ApproveScoreAsync(id, _currentUser);
        return NoContent();
    }

    // GET: api/score/export/class/{classId}
    [HttpGet("export/class/{classId:guid}")]
    [Authorize(Policy = "ScoreExport")]  // Admin, SchoolAdmin, SubjectAdmin, Teacher
    public async Task<IActionResult> ExportClassScores(Guid classId, [FromQuery] string format = "excel")
    {
        var fileBytes = await _scoreService.ExportScoresAsync(classId, format, _currentUser);
        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
    }
}
```

### 7.3. NewsController (Public + Manage)

```csharp
// Api/Controllers/NewsController.cs
[ApiController]
[Route("api/[controller]")]
public class NewsController : BaseController
{
    private readonly INewsService _newsService;

    public NewsController(INewsService newsService)
    {
        _newsService = newsService;
    }

    // GET: api/news — Public (Guest cũng xem được)
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetPublished([FromQuery] NewsQueryDTO query)
    {
        var result = await _newsService.GetPublishedNewsAsync(query);
        return Ok(result);
    }

    // POST: api/news
    [HttpPost]
    [Authorize(Policy = "NewsManage")]  // Admin, SchoolAdmin
    public async Task<IActionResult> Create([FromBody] NewsRequestDTO dto)
    {
        var result = await _newsService.CreateAsync(dto);
        return Created();
    }

    // DELETE: api/news/{id}
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "NewsManage")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _newsService.SoftDeleteAsync(id);
        return NoContent();
    }
}
```

---

## 8. Service Layer — Scope Filtering

### 8.1. Ví dụ UserService với scope check

```csharp
// Application/Services/UserService.cs (mở rộng)
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public async Task<UserResponseDTO?> GetByIdAsync(Guid id, ICurrentUser currentUser)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null || user.IsDeleted) return null;

        // Scope checks
        if (currentUser.IsInRole(RoleConstants.Admin))
            return MapToDTO(user);

        if (currentUser.IsInRole(RoleConstants.SchoolAdmin))
        {
            // SchoolAdmin chỉ xem user trong trường mình
            if (user.SchoolId != currentUser.SchoolId) return null;
            return MapToDTO(user);
        }

        if (currentUser.IsInRole(RoleConstants.SubjectAdmin))
        {
            if (user.SchoolId != currentUser.SchoolId) return null;
            return MapToDTO(user);
        }

        if (currentUser.IsInRole(RoleConstants.Teacher))
        {
            // Teacher xem được student trong các lớp mình dạy (đơn giản: check school)
            if (user.SchoolId != currentUser.SchoolId) return null;
            return MapToDTO(user);
        }

        if (currentUser.IsInRole(RoleConstants.Student))
        {
            // Student chỉ xem chính mình
            if (user.Id != currentUser.UserId) return null;
            return MapToDTO(user);
        }

        return null; // Guest hoặc không có quyền
    }

    public async Task<PagedResult<UserResponseDTO>> GetAllAsync(UserQueryDTO query, ICurrentUser currentUser)
    {
        // Ép filter scope cho non-admin
        if (!currentUser.IsInRole(RoleConstants.Admin))
        {
            query.SchoolId = currentUser.SchoolId ?? Guid.Empty;
        }

        var users = await _userRepository.GetPagedAsync(query);
        return MapToPagedResult(users);
    }
}
```

---

## 9. Security Best Practices

| # | Practice | Implementation |
|---|----------|----------------|
| 1 | **HTTPS everywhere** | `app.UseHttpsRedirection()` |
| 2 | **Token expiry ngắn** | 60 phút, refresh token (optional) |
| 3 | **Secret trong Vault/env** | `config["Jwt:Secret"]` từ环境变量, không commit |
| 4 | **Password hashing** | `PasswordHasher<T>` (đã có) |
| 5 | **Input validation** | FluentValidation / DataAnnotations trên DTOs |
| 6 | **Rate limiting** | `AddRateLimiter` trên login endpoint |
| 7 | **CORS restrictive** | Chỉ allow domain cụ thể, không `AllowAnyOrigin` ở production |
| 8 | **Audit log** | Ghi lại ai làm gì (Action, UserId, Timestamp) |
| 9 | **Principle of Least Privilege** | Default deny, grant permission tối thiểu cần thiết |
| 10 | **Token revocation** | blacklist token khi logout (Redis) hoặc short-lived + refresh |

---

## 10. Implementation Roadmap

### Phase 1 — Cơ bản (Tuần 1-2)
- [ ] Thêm entities: `Permissions`, `RolePermissions` + migration
- [ ] Seed Roles, Permissions, RolePermissions
- [ ] Implement `ICurrentUser` + `CurrentUserService`
- [ ] Implement JWT auth: `AuthenticationExtensions`, token generation trong `AuthService`
- [ ] Implement `PermissionHandler`, `ScopeHandler`
- [ ] Wire up `Program.cs`: `AddAuthentication`, `AddAuthorization`, handlers

### Phase 2 — API Security (Tuần 3-4)
- [ ] Apply `[Authorize(Policy=...)]` trên mọi controllers
- [ ] Implement scope filtering trong Services
- [ ] Test toàn bộ endpoints với từng role

### Phase 3 — Advanced (Tuần 5+)
- [ ] Audit logging
- [ ] Refresh token
- [ ] Rate limiting
- [ ] Admin UI quản lý Roles/Permissions (CRUD)
- [ ] Unit tests cho Authorization Handlers
- [ ] Integration tests cho Policy-based authorization

---

## 11. Testing Authorization

### 11.1. Unit Test cho PermissionHandler

```csharp
// Tests/Application.Tests/Authorization/PermissionHandlerTests.cs
public class PermissionHandlerTests
{
    [Theory]
    [InlineData("admin", true)]   // Admin always pass
    [InlineData("school_admin", true)]  // Has permission in claim
    [InlineData("student", false)] // No permission
    public async Task PermissionHandler_Should_Check_Correctly(string role, bool expectedResult)
    {
        // Arrange
        var currentUser = new Mock<ICurrentUser>();
        currentUser.Setup(c => c.IsInRole("admin")).Returns(role == "admin");
        currentUser.Setup(c => c.HasPermission("score.manage"))
            .Returns(role == "school_admin");

        var handler = new PermissionHandler(currentUser.Object, Mock.Of<IPermissionRepository>());
        var requirement = new PermissionRequirement("score.manage");
        var context = new AuthorizationHandlerContext(new[] { requirement }, new ClaimsPrincipal(), null);

        // Act
        await handler.HandleAsync(context);

        // Assert
        Assert.Equal(expectedResult, context.HasSucceeded);
    }
}
```

### 11.2. Integration Test

```csharp
// Tests/Api.Tests/Integration/AuthorizationTests.cs
public class ScoreAuthorizationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ScoreAuthorizationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Student_Cannot_Create_Score()
    {
        var client = _factory.CreateClient();
        // Login as student, get token
        var token = await LoginAs("student@test.com", "password");
        client.DefaultRequestHeaders.Authorization = new("Bearer", token);

        var response = await client.PostAsJsonAsync("/api/score", new { });
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Teacher_Can_Create_Score_For_Own_Subject()
    {
        var client = _factory.CreateClient();
        var token = await LoginAs("teacher@test.com", "password");
        client.DefaultRequestHeaders.Authorization = new("Bearer", token);

        var response = await client.PostAsJsonAsync("/api/score", new { /* valid data */ });
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}
```

---

## 12. Tóm Tắt

| Thành phần | Vị trí | Trách nhiệm |
|------------|--------|-------------|
| **Entities** | `Domain/Entities` | `Permissions`, `RolePermissions`, mở rộng `Roles`, `UserRoles` |
| **Enums** | `Domain/Enums` | (optional) `Resource`, `Action` enum thay vì string |
| **DTOs** | `Application/DTOs` | `RoleDTO`, `PermissionDTO`, `RolePermissionDTO` |
| **Constants** | `Application/Constants` | `PermissionConstants`, `RoleConstants`, `PolicyConstants` |
| **Requirements** | `Application/Authorization/Requirements` | `PermissionRequirement`, `ScopeRequirement` |
| **Handlers** | `Application/Authorization/Handlers` | `PermissionHandler`, `ScopeHandler` |
| **Services** | `Application/Services` | `AuthService` (token gen), `RoleService`, `UserService` (scope filter) |
| **Repositories** | `Infrastructure/Repositories` | `RoleRepository`, `PermissionRepository` |
| **DbContext** | `Infrastructure/Persistence` | DbSets, Fluent Config, Seed Data |
| **Extensions** | `Api/Extensions` | `AuthenticationExtensions`, `AuthorizationExtensions` |
| **Controllers** | `Api/Controllers` | `[Authorize(Policy=...)]`, inject `ICurrentUser` |
| **Middleware** | `Api/Middleware` | `ExceptionMiddleware`, (optional) `AuditMiddleware` |
| **Program.cs** | `Api/Program.cs` | `AddAuthentication`, `AddAuthorization`, `UseAuthentication`, `UseAuthorization` |

**Key Takeaway:**
- **RBAC đầy đủ** = Roles + Permissions tables + Policy-based authorization trong .NET
- **Scope-based** = JWT claims (`school_id`, `class_id`) + Service layer filtering
- **Admin** = bypass mọi permission check
- **Default Deny** = không grant gì → không có quyền
- **Test** = Unit test handlers + Integration test endpoints với từng role
