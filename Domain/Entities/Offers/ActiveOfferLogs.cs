using Domain.Entities.Chats;
using Domain.Entities.Shared;
using Domain.Enums.Status;
using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.Entities.Offers
{
    public class ActiveOfferLogs:Audits
    {
        public Guid ID { get; set; }
        public ActiveOfferStatus Status { get; set; }
        public string? Notes { get; set; }
       
        public Guid? PublishedOfferID { get; set; }
        public Guid? CustomOfferID { get; set; }
        public bool? IsPublishedOfferActive { get; set; }
        public bool? IsCustomOfferActive { get; set; }

        public Chat Chat { get; set; }
        [ForeignKey("Chat")]
        public int ChatID { get; set; }
        // ef core when reading data from DBSet of the class it creates new obj(calling the parameterless ctor only even if it's a private constructor)
        // mapping data from the DB to the obj properties so we need to have a parameterless constructor for ef core to work properly

        public void CloseState() =>
            UpdatedAt = DateTime.Now;
    }
}
