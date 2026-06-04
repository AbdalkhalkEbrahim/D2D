using Domain.Entities.Offers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

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
