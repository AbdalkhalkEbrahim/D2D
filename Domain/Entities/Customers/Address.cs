using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Customers
{
    public class Address
    {
        public int ID { get; set; }
        public string AppartmentNo { get; set; }
        public int? BuildingNumber { get; set; }
        public string Street { get; set; }
        public string? District { get; set; }
        public string City { get; set; }
        public string Goverate { get; set; }
        public bool Selected { get; set; }

        public  virtual Customer Customer { get; set; }
        [ForeignKey("Customer")]
        public string CustomerID { get; set; }


        public override bool Equals(object? obj)
        {
            if (obj == null) throw new ArgumentNullException();
            if (ReferenceEquals(this, obj)) return true;
            var address = (Address)obj;
            return (address.AppartmentNo == this.AppartmentNo && address.Street == this.Street && address.City == this.City && address.Goverate == this.Goverate&&BuildingNumber==address.BuildingNumber&&District==address.District);
        }
        override public int GetHashCode()
        {
            return HashCode.Combine(AppartmentNo, Street, City, Goverate,District,BuildingNumber);
        }
        
    }
}
