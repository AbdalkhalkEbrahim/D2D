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
    public class CustomerDesignConfiguration : IEntityTypeConfiguration<CustomerDesign>
    {
        public void Configure(EntityTypeBuilder<CustomerDesign> builder)
        {

            builder.HasMany(cd => cd.DesignImages)
                    .WithOne(d => d.CustomerDesign)
                    .HasForeignKey(di => di.CustomerDesignID)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
