namespace Domain.Entities.Shared
{
    public abstract class Offer:Audits
    {
        public Guid ID { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string TargetAudience { get; set; }
        public bool Gender {  get; set; }
        public List<string> Colors { get; set; }
        public string? Material { get; set; }
        public string? PrintingType { get; set; }
        public List<string> Sizes { get; set; }
        
        public string? SizesFile { get; set; }
        public int Duration { get; set; }
        public int Amount { get; set; }
        public decimal MaxPrice { get; set; }
        public bool IsActive { get; set; } = false;
        protected Offer()
        {
            Sizes = new List<string>();
            Colors = new List<string>();
        }
    }
}
