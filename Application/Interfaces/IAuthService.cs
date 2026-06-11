using Application.Response;
using Domain.DTOs;
using Domain.Entities.Shared;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        public Task<Result<TokenDTO>> GenerateAccessToken(User user);
        public Task<Result<TokenDTO>> GenerateRefreshToken(string userId);
        public Task<Result> RevokeRefreshToken(string token);
        public Task<Result<JwtToken>> JwtGenratedToken(string refreshToken);
    }
}
