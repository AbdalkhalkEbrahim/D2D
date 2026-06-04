using Domain.Entities.Designers;
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
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.HasOne(r => r.Producer)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(r => r.ProducerID)
                    .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(r => r.Designer)
                    .WithMany(d => d.Reports)
                    .HasForeignKey(r => r.DesignerID)
                    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
