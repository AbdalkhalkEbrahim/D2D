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
            modelBuilder.Entity<Offer>().UseTpcMappingStrategy();
            modelBuilder.Entity<CustomerOffer>().UseTpcMappingStrategy();
            modelBuilder.Entity<ProducerOffer>().UseTpcMappingStrategy();
            modelBuilder.Entity<Design>().UseTpcMappingStrategy();
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
        }
    }
}