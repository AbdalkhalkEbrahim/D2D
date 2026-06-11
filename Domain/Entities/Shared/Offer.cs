namespace Domain.Entities.Shared
{
    public abstract class Offer:Audits
    {
        public Guid ID { get; set; }
        public required string Material { get; set; }
        public int Duration { get; set; }
        public int Amount { get; set; }
        public decimal MaxPrice { get; set; }
        public bool IsActive { get; set; }

    }
}
