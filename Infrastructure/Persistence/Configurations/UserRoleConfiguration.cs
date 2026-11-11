using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> e)
    {
        e.ToTable("UserRoles");
        e.HasOne(x => x.User).WithMany(u => u.Roles).HasForeignKey(e => e.UserId);
        e.HasOne(x => x.Role).WithMany(u => u.Users).HasForeignKey(e => e.RoleId);
    }
}
