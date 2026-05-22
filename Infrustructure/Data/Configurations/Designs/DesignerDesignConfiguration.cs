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
    public class DesignerDesignConfiguration : IEntityTypeConfiguration<DesignerDesign>
    {
        public void Configure(EntityTypeBuilder<DesignerDesign> builder)
        {
            builder.HasMany(dd => dd.DesignImages)
                    .WithOne(d => d.DesignerDesign)
                    .HasForeignKey(di => di.DesignerDesignID)
                    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
