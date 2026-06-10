using Domain.Enums;

namespace Domain.DTOs
{
    public class UserRegisterationResponse
    {
        public required string UserId {  get; set; }
        public required string Email { get; set; }
        public UserType UserType { get; set; }
        public string NextStep { get; } = "SentOtp";
    }
}
