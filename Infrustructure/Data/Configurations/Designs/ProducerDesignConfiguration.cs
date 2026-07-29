using Domain.Entities.Designs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
