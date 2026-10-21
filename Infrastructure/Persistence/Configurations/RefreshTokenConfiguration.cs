using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders; 

namespace Infrastructure.Persistence.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> e)
        {
            e.ToTable("RefreshToken");
            e.HasIndex("Token");
            e.HasOne(x=>x.User).WithMany(u=>u.RefreshTokens).HasForeignKey(x=>x.UserId);
        }
    }
}