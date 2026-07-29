 using Application.Response;
using Domain.DTOs.AuthDtos;
using Domain.Entities.Shared;
using Domain.Enums.Types;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        public Task<TokenDTO> GenerateAccessToken(User user);
        public Task<Result<TokenDTO>> GenerateRefreshToken(string userId);
        public Task<Result> RevokeRefreshToken(string token);
        public Task<Result<JwtToken>> JwtGenratedToken(string refreshToken);
        public Task<string> AnonymousName(UserType userType);

    }
}
