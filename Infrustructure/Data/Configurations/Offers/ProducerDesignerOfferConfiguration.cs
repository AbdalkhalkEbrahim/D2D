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
    public class ProducerDesignerOfferConfiguration : IEntityTypeConfiguration<ProducerDesignerOffer>
    {
        public void Configure(EntityTypeBuilder<ProducerDesignerOffer> builder)
        {
            builder.HasOne(pdo => pdo.DesignerDesign)
                 .WithMany(dd => dd.ProducerDesignerOffers)
                 .HasForeignKey(pdo => pdo.DesignerDesignID)
                 .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
