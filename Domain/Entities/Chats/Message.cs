using Domain.Entities.Shared;

namespace Domain.Entities.Chats
{
    public class Message:Audits
    {
        public int ID { get; set; }
        public string? Content { get; set; }
        public required string SenderID { get; set; }
        public required string RecieverID { get; set; }
        public required Chat Chat { get; set; }

        public void CreatedBy(string SenderID)
        {
            this.SenderID = SenderID;
            if (SenderID == Chat.ProducerID)
                RecieverID = Chat.CustomerID;
            else
                RecieverID = Chat.ProducerID;
        }

    }
}
