using Domain.Entities.Designs;
using Domain.Entities.Producers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configurations.Users
{
    public class ProducerConfiguration : IEntityTypeConfiguration<Producer>
    {
        public void Configure(EntityTypeBuilder<Producer> builder)
        {
            builder.HasMany(p => p.Favourite)
              .WithMany(d => d.FavoritedBy)
              .UsingEntity<Dictionary<string, object>>(
                  "DesignerDesignProducer",
                  j => j.HasOne<DesignerDesign>().WithMany().HasForeignKey("FavouriteID").OnDelete(DeleteBehavior.Cascade),
                  j => j.HasOne<Producer>().WithMany().HasForeignKey("FavoritedByID").OnDelete(DeleteBehavior.NoAction)
              );

        }
    }
}
