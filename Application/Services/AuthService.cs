using Domain.Entities.Shared;
using Application.Interfaces;
using Domain.Settings;
using Application.Response;
using Infrastructure.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain.DTOs.AuthDtos;

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
        public async Task<Result<TokenDTO>> GenerateAccessToken(User user)
        {
            user = await _userManager.FindByIdAsync(user.Id);
            if (user is null)
                return Result<TokenDTO>.Failure(Messages.NotFound.WithTarget("User"));

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

            var jwtsecurity = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials
             );

            return Result<TokenDTO>.Success(new TokenDTO{
                UserID = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtsecurity),
                ExpiresAt = expiration
            };
        }

        public async Task<Result<TokenDTO>> GenerateRefreshToken(string userId)
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
            var generatedRefreshTokenEntity = new RefreshToken
            {
                Token = refreshToken.Token,
                ExpiresAt = refreshToken.ExpiresAt,
                IsRevoked = false,
                UserID = userId
            };

            var user = _context.Users.Include(u => u.RefreshTokens).FirstOrDefault(u => u.Id == userId) ;
            if(user == null)
                return Result<TokenDTO>.Failure(Messages.NotFound.WithTarget("User"));
            user.RefreshTokens?.Add(generatedRefreshTokenEntity);
            await _userManager.UpdateAsync(user);

            return Result<TokenDTO>.Success(refreshToken);
        }

        public async Task<Result<JwtToken>> JwtGenratedToken(string refreshToken)
        {
            var user = await _context.Users.Include(u => u.RefreshTokens)
               .SingleOrDefaultAsync(u => u.RefreshTokens!.Any(r => r.Token == refreshToken));
            if (user == null)
                return Result<JwtToken>.Failure(Messages.Expired.WithTarget("Token"));
               

            var existingRefreshToken = user.RefreshTokens!.Single(x => x.Token == refreshToken);

            if (existingRefreshToken.IsRevoked || existingRefreshToken.ExpiresAt < DateTime.UtcNow)
                return Result<JwtToken>.Failure(Messages.Expired.WithTarget("Token"));

            existingRefreshToken.IsRevoked = true;

            var newRefreshToken = (await GenerateRefreshToken(user.Id)).Value;

            var jwt = await GenerateAccessToken(user);
            return Result<JwtToken>.Success(new JwtToken   
            {
                UserID = user.Id,
                AccessToken = jwt.Token,
                RefreshToken = newRefreshToken.Value.Token,
                AccessTokenExpiresAt = jwt.ExpiresAt,
                RefreshTokenExpiresAt = newRefreshToken.Value.ExpiresAt
            });
        }

        public async Task<Result> RevokeRefreshToken(string token)
        {
            var existingtoken = _context.RefreshTokens.FirstOrDefault(t => t.Token == token);
            if (existingtoken != null)
            {
                existingtoken.IsRevoked = true;
                _context.Update(existingtoken);
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            return Result.Failure(Messages.NotFound.WithTarget("Default"));
        }
    }
}