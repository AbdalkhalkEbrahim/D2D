namespace Domain.DTOs.AuthDtos
{
    public class TokenDTO
    {
        public required string UserID { get; set; }
        public required string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
