namespace Domain.DTOs.OfferDtos
{
    public class ProducerOfferResponse
    {
        public string ProducerId { get; set; }
        public Guid ProducerOfferId { get; set; }
        public string ProducerAnnonName { get; set; }
        public List<string> ImageUrl { get; set; }
        public decimal Price { get; set; }
        public string OfferStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Dictionary<string,Tuple<string,int>> Reviews { get; set; }
        public Dictionary<string, string> Gallery { get; set; }
        public double Rate { get; set; }
        public string Name { get; set; }
        public decimal Diposit { get; set; }
        public int DeliveryTime { get; set; }
        public Dictionary<string, Tuple<int, int>> Steps { get; set; }
    }
}
