using Domain.Entities.Offers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations.Offers
{
    public class ProducerOfferConfiguration : IEntityTypeConfiguration<ProducerOffer>
    {
        public void Configure(EntityTypeBuilder<ProducerOffer> builder)
        {
            //builder.UseTpcMappingStrategy();

            builder.HasOne(po => po.Producer)
                    .WithMany(p => p.ProducerOffers)
                    .HasForeignKey(po => po.ProducerID)
                    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
