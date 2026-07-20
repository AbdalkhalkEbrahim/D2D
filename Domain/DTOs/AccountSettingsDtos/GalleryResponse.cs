namespace Domain.DTOs.AccountSettingsDtos
{
    public class GalleryResponse
    {
        public Dictionary<(Guid,string),List<string>> Design {  get; set; }
        public string Notes { get; set; }
        public string Location  { get; set; }
        public string Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
