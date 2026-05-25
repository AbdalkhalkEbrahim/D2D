using Domain.DTOs;
using Domain.Entities.Shared;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Settings;
using Infrastructure.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<User> _userManager;
        private readonly D2DContext _context;
        public AuthService(IOptions<JwtSettings> options, UserManager<User> userManager, D2DContext context)
        {
            _jwtSettings = options.Value;
            _userManager = userManager;
            _context = context;
        }
        public async Task<TokenDTO> GenerateAccessToken(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes);
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role,userRoles.FirstOrDefault()??"User")

            };

            var jwtsecurity= new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials
             );

            return new TokenDTO
            {
                UserID = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtsecurity),
                ExpiresAt = expiration
            };  
        }

        public TokenDTO GenerateRefreshToken(string userId)
        {
            var randomNumberGenerator = RandomNumberGenerator.Create();
            var randomBytes = new byte[64];
            randomNumberGenerator.GetBytes(randomBytes);
            var token = Convert.ToBase64String(randomBytes);

            var refreshToken = new TokenDTO
            {
                UserID = userId,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
            };

            return refreshToken;
        }

        public async Task<JwtToken> JwtGenratedToken(string refreshToken)
        {
            var user = await _userManager.Users
               .SingleOrDefaultAsync(u => u.RefreshTokens!.Any(r => r.Token == refreshToken))
               ?? throw new SecurityTokenException("Invalid or expired refresh token");

            var existingRefreshToken = user.RefreshTokens!.Single(x => x.Token == refreshToken);

            if (existingRefreshToken.IsRevoked||existingRefreshToken.ExpiresAt<DateTime.Now)
                throw new SecurityTokenException("Invalid or expired refresh token");

            existingRefreshToken.IsRevoked = true;

            var newRefreshToken = GenerateRefreshToken(user.Id);
            user.RefreshTokens ??= [];
            var newRefreshTokenEntity = new RefreshToken
            {
                Token = newRefreshToken.Token,
                ExpiresAt = newRefreshToken.ExpiresAt,
                IsRevoked = false,
                UserID = user.Id
            };

            user.RefreshTokens.Add(newRefreshTokenEntity);

            await _userManager.UpdateAsync(user);

            var jwt = await GenerateAccessToken(user);
            return new JwtToken
            {
                UserID = user.Id,
                AccessToken = jwt.Token,
                RefreshToken = newRefreshToken.Token,
                AccessTokenExpiresAt = jwt.ExpiresAt,
                RefreshTokenExpiresAt = newRefreshToken.ExpiresAt
            };
        }

        public async Task RevokeRefreshToken(string token)
        {
            var existingtoken = _context.RefreshTokens.FirstOrDefault(t => t.Token == token);
            if (existingtoken != null)
            {
                existingtoken.IsRevoked = true;
                _context.Update(existingtoken);
                await _context.SaveChangesAsync();
            }

        }
    }
}
