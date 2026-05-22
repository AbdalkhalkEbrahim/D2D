using Domain.Entities.Producers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

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
