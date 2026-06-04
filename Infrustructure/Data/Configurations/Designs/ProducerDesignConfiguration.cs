using Domain.Entities.Designs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configurations.Designs
{
    internal class ProducerDesignConfiguration : IEntityTypeConfiguration<ProducerDesign>
    {
        public void Configure(EntityTypeBuilder<ProducerDesign> builder)
        {

            builder.HasMany(pd => pd.DesignImages)
                    .WithOne(d => d.ProducerDesign)
                    .HasForeignKey(di => di.ProducerDesignID)
                    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
