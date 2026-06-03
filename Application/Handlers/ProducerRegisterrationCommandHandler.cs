using Application.Commands;
using Domain.DTOs;
using Domain.Entities.Producers;
using Domain.Entities.Shared;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Data.Context;
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
        private readonly D2DContext _context;
        public ProducerRegisterrationCommandHandler(UserManager<User> userManager, IUploadService uploadService, IAuthService authService, D2DContext context)
        {
            _userManager = userManager;
            _uploadService = uploadService;
            _authService = authService;
            _context = context;
        }
        public async Task<JwtToken> Handle(ProducerRegisterrationCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.ProducerId);
            if (user == null || !user.EmailConfirmed || user.UserType != UserType.Producer)
                throw new Exception("Invalid request");

            var licenseUrls = new List<LicenseVerification>();
            foreach(var file in request.LicenseUrls)
            {
                var url = await _uploadService.UploadFileAsync(file);
                licenseUrls.Add(new LicenseVerification { LicenseUrl = url });
            }

            Producer producer = (Producer) user;
            producer.FrontImageID = await _uploadService.UploadFileAsync(request.FrontImageID);
            producer.BackImageID = await _uploadService.UploadFileAsync(request.BackImageID);
            producer.PersonalImage = await _uploadService.UploadFileAsync(request.PersonalImage);
            producer.LicenseVerifications = licenseUrls;
            _context.Producers.Update(producer);
            await _context.SaveChangesAsync();
            //await _userManager.UpdateAsync(producer);

            var result = new Dictionary<string, string>
            {
                { "FrontImageID", producer.FrontImageID },
                { "BackImageID", producer.BackImageID },
                { "PersonalImage", producer.PersonalImage },
                { "LicenseUrls", string.Join(", ", licenseUrls.Select(l => l.LicenseUrl)) }
            };
            var AccessToken = await _authService.GenerateAccessToken(producer);
            var refreshToken = await _authService.GenerateRefreshToken(producer.Id);
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
