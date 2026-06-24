namespace Domain.DTOs.OfferDtos
{
    public class ProducerOfferResponse
    {
        public string ProducerId { get; set; }
        public string ProducerAnnonName { get; set; }
        public decimal Price { get; set; }
        public string OfferStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public Dictionary<string,string> Reviews { get; set; }
        public double Rate { get; set; }

    }
}
