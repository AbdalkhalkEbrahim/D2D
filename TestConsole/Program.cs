using Domain.Entities.Customers;
namespace TestConsole
{

    internal class Program
    {
        static void Main(string[] args)
        {
            Address address = new Address() { AppartmentNo = "df", Street = "sdsds", City = "euyrue", Goverate = "kjwidwid"};
            Address address2 = new Address() { AppartmentNo = "df", Street = "sdsds", City = "euyrue", Goverate = "kjwidwid" };
            Console.WriteLine(address.Equals(address2));
        }
    }
}
