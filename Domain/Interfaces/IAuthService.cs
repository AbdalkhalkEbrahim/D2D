using Domain.DTOs;
using Domain.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IAuthService
    {
        public Task<TokenDTO> GenerateAccessToken(User user);
        public TokenDTO GenerateRefreshToken(string userId);
        public Task RevokeRefreshToken(string token);
        public Task<JwtToken> JwtGenratedToken(string refreshToken);
    }
}
