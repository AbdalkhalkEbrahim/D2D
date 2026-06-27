using Domain.Entities.Shared;
using Domain.Enums.Status;
namespace Domain.Entities.Offers
{
    public class ActiveOfferLogs:Audits
    {
        public Guid ID { get; set; }
        public ActiveOfferStatus Status { get; set; }
        public string? Notes { get; set; }
       
        public Guid OfferID { get; set; }
        public bool IsOfferActive { get; set; }


        // ef core when reading data from DBSet of the class it creates new obj(calling the parameterless ctor only even if it's a private constructor)
        // mapping data from the DB to the obj properties so we need to have a parameterless constructor for ef core to work properly
        private ActiveOfferLogs() { }

        public ActiveOfferLogs(Guid _offerID,ActiveOfferStatus _status)
        {
            Status = _status;
            OfferID = _offerID;
           
        }
        public void CloseState() =>
            UpdatedAt = DateTime.Now;
    }
}
