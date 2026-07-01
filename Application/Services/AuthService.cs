using Application.Interfaces;
using Application.Response;
using Domain.DTOs.AuthDtos;
using Domain.Entities.Shared;
using Domain.Enums.Types;
using Domain.Settings;
using Infrastructure.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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
            var userRoles = await _userManager.GetRolesAsync(user);//
            var expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes);
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role,user.UserType.ToString())

            };

            var jwtsecurity = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials
             );

            return new TokenDTO{
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

            var user = await _context.Users.AnyAsync(u => u.Id == userId) ;
            if(!user)
                return Result<TokenDTO>.Failure(Messages.NotFound.WithTarget("User"));
            generatedRefreshTokenEntity.UserID = userId;
            _context.RefreshTokens.Add(generatedRefreshTokenEntity);
            await _context.SaveChangesAsync();

            return Result<TokenDTO>.Success(refreshToken);
        }

        public async Task<Result<JwtToken>> JwtGenratedToken(string refreshToken)
        {
            var existingRefreshToken = await _context.RefreshTokens.Include(t => t.User).FirstOrDefaultAsync(t=>t.Token == refreshToken);
            
            if (existingRefreshToken is null || existingRefreshToken.IsRevoked || existingRefreshToken.ExpiresAt < DateTime.UtcNow)
                return Result<JwtToken>.Failure(Messages.Expired.WithTarget("Token"));

            existingRefreshToken.IsRevoked = true;

            var newRefreshToken = (await GenerateRefreshToken(existingRefreshToken.UserID)).Value;

            var jwt = await GenerateAccessToken(existingRefreshToken.User);
            await _context.SaveChangesAsync(); 
            return Result<JwtToken>.Success(new JwtToken   
            {
                UserID = existingRefreshToken.UserID,
                AccessToken = jwt.Token,
                RefreshToken = newRefreshToken.Token,
                AccessTokenExpiresAt = jwt.ExpiresAt,
                RefreshTokenExpiresAt = newRefreshToken.ExpiresAt
            });
        }

        public async Task<Result> RevokeRefreshToken(string token)
        {
            int updatedRows = await _context.RefreshTokens
                .Where(x => x.Token == token
                         && !x.IsRevoked
                         && x.ExpiresAt > DateTime.UtcNow)
                .ExecuteUpdateAsync(setters => setters.SetProperty(t => t.IsRevoked, true));
            if(updatedRows == 0)
                return Result.Failure(Messages.Expired.WithTarget("Token"));
            return Result.Success();

        }
        public async Task<string> AnonymousName(UserType userType)
        {
            var counters = await _context.SystemCounters.FirstOrDefaultAsync();
            int AnonCounter = (userType) switch
            {
                UserType.Customer => counters.CustomerCounter = counters.CustomerCounter + 1,
                UserType.Producer => counters.ProducerCounter = counters.ProducerCounter + 1,
                UserType.Designer => counters.DesignerCounter = counters.DesignerCounter + 1,
                _ => 0
            };
            _context.Update(counters);
            await _context.SaveChangesAsync();
            AnonCounter++;
            string leadingZeros = new string('0', 6 - AnonCounter.ToString().Length);
            return $"Anon{leadingZeros}{AnonCounter}_{userType.ToString()}";
        }
    }
}