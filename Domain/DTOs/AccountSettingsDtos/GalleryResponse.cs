namespace Domain.DTOs.AccountSettingsDtos
{
    public class GalleryResponse
    {
        public Guid DesignId { get; set; }
        public string ProducerAnonName { get; set; } 
        public List<Tuple<int,string>> Images {  get; set; }
        public string Notes { get; set; }
        public string Location  { get; set; }
        public string Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
