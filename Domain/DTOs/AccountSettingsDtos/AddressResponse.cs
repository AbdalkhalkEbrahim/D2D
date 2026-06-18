namespace Domain.DTOs.AccountSettingsDtos
{
    public class AddressResponse
    {
        public int ID { get; set; }
        public string CustomerId { get; set; }
        public string AppartmentNo { get; set; }
        public int? BuildingNumber { get; set; }
        public string Street { get; set; }
        public string? District { get; set; }
        public string City { get; set; }
        public string Goverate { get; set; }
        public bool Selected { get; set; }
    }
}
