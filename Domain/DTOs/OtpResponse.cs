using Domain.Enums.Types;

namespace Domain.DTOs
{
    public class OtpResponse
    {
        public required string UserId { get; set; }
        public UserType UserType { get; set; }
        public string Code { get; set; }
    }
}
