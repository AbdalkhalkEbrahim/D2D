
namespace Domain.DTOs.AccountSettingsDtos
{
    public class AddressRequst
    {
        public string AppartmentNo { get; set; }
        public int? BuildingNumber { get; set; }
        public string Street { get; set; }
        public string? District { get; set; }
        public string City { get; set; }
        public string Goverate { get; set; }
    }
}
