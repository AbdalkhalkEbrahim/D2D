using Domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations.Users
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //builder.HasMany(u => u.RefreshTokens)
            //   .WithOne(r => r.User)
            //   .HasForeignKey(r => r.UserID);
            builder.UseTptMappingStrategy();
        }
    }
}
