using Application.Commands;
using Domain.DTOs;
using Domain.Entities.Producers;
using Domain.Entities.Shared;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    internal class ProducerRegisterrationCommandHandler : IRequestHandler<ProducerRegisterrationCommand, JwtToken>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUploadService _uploadService;
        private readonly IAuthService _authService;
        public ProducerRegisterrationCommandHandler(UserManager<User> userManager, IUploadService uploadService, IAuthService authService)
        {
            _userManager = userManager;
            _uploadService = uploadService;
            _authService = authService; 
        }
        public async Task<JwtToken> Handle(ProducerRegisterrationCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.ProducerId);
            if (user == null || !user.EmailConfirmed || user.UserType != UserType.Producer)
                throw new Exception("Invalid request");

            var licenseUrls = new List<LicenseVerification>();
            request.LicenseUrls.ForEach(async file =>
            {
                var url = await _uploadService.UploadFileAsync(file);
                licenseUrls.Add(new LicenseVerification { LicenseUrl = url });
            });
            var producer = new Producer
            {
                Id = user.Id,
                FrontImageID = await _uploadService.UploadFileAsync(request.FrontImageID),
                BackImageID = await _uploadService.UploadFileAsync(request.BackImageID),
                PersonalImage = await _uploadService.UploadFileAsync(request.PersonalImage),
                LicenseVerifications = licenseUrls
            };
            await _userManager.UpdateAsync(producer);
            
            var result = new Dictionary<string, string>
            {
                { "FrontImageID", producer.FrontImageID },
                { "BackImageID", producer.BackImageID },
                { "PersonalImage", producer.PersonalImage },
                { "LicenseUrls", string.Join(", ", licenseUrls.Select(l => l.LicenseUrl)) }
            };
            var AccessToken = await _authService.GenerateAccessToken(producer);
            var refreshToken = _authService.GenerateRefreshToken(producer.Id);
            return new JwtToken
            {
                UserID = producer.Id,
                AccessToken = AccessToken.Token,
                RefreshToken = refreshToken.Token,
                AccessTokenExpiresAt = AccessToken.ExpiresAt,
                RefreshTokenExpiresAt = refreshToken.ExpiresAt,
                Data = result,

            };
        }
    }
}
