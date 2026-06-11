using Domain.Entities.Chats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations.Chats
{
    public class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.HasOne(c => c.Customer)
                    .WithMany(u => u.Chats)
                    .HasForeignKey(c => c.CustomerID)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Producer)
                    .WithMany(u => u.Chats)
                    .HasForeignKey(c => c.ProducerID)
                    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
