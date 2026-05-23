using Domain.Entities.Chats;
using Domain.Entities.Chats.AiModel;
using Domain.Entities.Customers;
using Domain.Entities.Designers;
using Domain.Entities.Designs;
using Domain.Entities.Offers;
using Domain.Entities.Payment;
using Domain.Entities.Producers;
using Domain.Entities.Shared;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Data.Context
{
    public class D2DContext : IdentityDbContext<User>
    {
        public D2DContext(DbContextOptions<D2DContext> options) : base(options) { }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ActiveOfferLogs> ActiveOfferLogs { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Designer> Designers { get; set; }
        public DbSet<DesignVerification> DesignVerifications { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<CustomerDesign> CustomerDesigns { get; set; }
        public DbSet<ProducerDesign> ProducerDesigns { get; set; }
        public DbSet<DesignerDesign> DesignerDesigns { get; set; }
        public DbSet<DesignImage> DesignImages { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<LicenseVerification> LicenseVerifications { get; set; }
        public DbSet<Escrow> Escrows { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ProducerCustomerOffer> ProducerCustomerOffers { get; set; }
        public DbSet<CustomerPublishedOffer> CustomerPublishedOffers { get; set; }
        public DbSet<ProducerDesignerOffer> ProducerDesignerOffers { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ModelChat> ModelChats { get; set; }
        public DbSet<ModelChatMessage> ModelChatMessages { get; set; }
        public DbSet<Otp> Otps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CustomerOffer>().UseTpcMappingStrategy();
            modelBuilder.Entity<ProducerOffer>().UseTpcMappingStrategy();
            modelBuilder.Entity<Design>().UseTpcMappingStrategy();
         
            // ============================================================================
            // 1. TPH (Table-Per-Hierarchy) Configuration for CustomerOffer Tree
            // ============================================================================
            modelBuilder.Entity<CustomerOffer>()
                .HasDiscriminator<int>("Discriminator")
                .HasValue<CustomerPublishedOffer>(0)
                .HasValue<CustomerCustomOffer>(1);

            // ============================================================================
            // 2. TPH (Table-Per-Hierarchy) Configuration for ProducerOffer Tree
            // ============================================================================
            modelBuilder.Entity<ProducerOffer>()
                .HasDiscriminator<int>("Discriminator")
                .HasValue<ProducerCustomerOffer>(0)
                .HasValue<ProducerDesignerOffer>(1);

            // ============================================================================
            // 3. CustomerPublishedOffer Relationships (1-to-Many)
            // ============================================================================
            modelBuilder.Entity<ProducerCustomerOffer>()
                .HasOne(p => p.CustomerOffer)
                .WithMany(c => c.ProducerCustomerOffers)
                .HasForeignKey(p => p.CustomerOfferID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // ============================================================================
            // 4. CustomerCustomOffer Relationships (1-to-1)
            // ============================================================================

            // A. 1-to-1 Relationship with ProducerCustomerOffer
            modelBuilder.Entity<ProducerCustomerOffer>()
                .HasOne(p => p.CustomerOffer)
                .WithOne(c => c.ProducerCustomerOffer)
                .HasForeignKey<ProducerCustomerOffer>(p => p.CustomerOfferID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            // B. 1-to-1 Relationship with ProducerDesignerOffer
/*            modelBuilder.Entity<ProducerDesignerOffer>()
                .HasOne(p => p.DesignerDesign)
                .WithMany(d => d.ProducerDesignerOffers)
                .HasForeignKey<ProducerDesignerOffer>(p => p.DesignerDesignID)
                .OnDelete(DeleteBehavior.ClientSetNull);*/

            // ============================================================================
            // 5. Filtered Indexes for Performance and Data Integrity
            // ============================================================================

            // Performance filtered indexes on the unified CustomerOffers table
            modelBuilder.Entity<CustomerOffer>()
                .HasIndex(c => c.ID)
                .HasDatabaseName("IX_CustomerOffer_Published")
                .HasFilter("[Discriminator] = 0");

            modelBuilder.Entity<CustomerOffer>()
                .HasIndex(c => c.ID)
                .HasDatabaseName("IX_CustomerOffer_Custom")
                .HasFilter("[Discriminator] = 1");

            // Unique filtered index to enforce 1-to-1 integrity for CustomerCustomOffer inside ProducerCustomerOffer hierarchy
            modelBuilder.Entity<ProducerCustomerOffer>()
                .HasIndex(p => p.CustomerOfferID)
                .HasDatabaseName("IX_Unique_CustomerCustom_To_ProducerCustomer")
                .IsUnique()
                .HasFilter("[Discriminator] = 0 AND [CustomerCustomOfferId] IS NOT NULL");

            // Unique filtered index to enforce 1-to-1 integrity for CustomerCustomOffer inside ProducerDesignerOffer hierarchy
            modelBuilder.Entity<ProducerDesignerOffer>()
                .HasIndex(p => p.DesignerDesignID)
                .HasDatabaseName("IX_Unique_CustomerCustom_To_ProducerDesigner")
                .IsUnique()
                .HasFilter("[Discriminator] = 1 AND [CustomerCustomOfferId] IS NOT NULL");
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
        }
    }
}