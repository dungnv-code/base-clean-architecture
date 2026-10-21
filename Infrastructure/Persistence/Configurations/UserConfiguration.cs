using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> e)
        {
            e.ToTable("Users");
            e.HasIndex(x => x.Email).IsUnique();
        }
    }

    // public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    // {
    //     public void Configure(EntityTypeBuilder<UserRole> e)
    //     {
    //         e.ToTable("UserRoles");
    //         e.HasOne(x => x.User).WithMany(u => u.Roles).HasForeignKey(x => x.UserId);
    //         e.HasOne(x => x.Role).WithMany(r => r.Users).HasForeignKey(x => x.RoleId);
    //     }
    // }

    // public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    // {
    //     public void Configure(EntityTypeBuilder<RefreshToken> e)
    //     {
    //         e.ToTable("RefreshTokens");
    //         e.HasIndex(x => x.Token).IsUnique();
    //         e.HasOne(x => x.User).WithMany(u => u.RefreshTokens).HasForeignKey(x => x.UserId);
    //     }
    // }
}