namespace Domain.DTOs
{
    public class JwtToken
    {
        public string UserID { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AccessTokenExpiresAt { get; set; }
        public DateTime RefreshTokenExpiresAt { get; set; }
        public dynamic? Data { get; set; }
    }
}
