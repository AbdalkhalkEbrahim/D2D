using Domain.Entities.Producers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasOne(r => r.Customer)
                    .WithMany(c => c.Reviews)
                    .HasForeignKey(r => r.CustomerID)
                    .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(r => r.Producer)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(r => r.ProducerID)
                    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
