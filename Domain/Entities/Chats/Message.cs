using Domain.Entities.Shared;

namespace Domain.Entities.Chats
{
    public class Message:Audits
    {
        public int ID { get; set; }
        public string? Content { get; set; }
        public required Guid SenderID { get; set; }
        public required Guid RecieverID { get; set; }
        public required Chat Chat { get; set; }

        public void CreatedBy(Guid SenderID)
        {
            this.SenderID = SenderID;
            if (SenderID == Chat.ProducerID)
                RecieverID = Chat.CustomerID;
            else
                RecieverID = Chat.ProducerID;
        }

    }
}
