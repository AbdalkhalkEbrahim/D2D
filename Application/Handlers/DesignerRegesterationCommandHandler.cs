using Application.Commands;
using Application.Services;
using Domain.DTOs;
using Domain.Entities.Designers;
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
    public class DesignerRegesterationCommandHandler : IRequestHandler<DesignerRegesterationCommand, JwtToken>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUploadService _uploadService;
        private readonly D2DContext _context;
        private readonly IAuthService _authService;
        public DesignerRegesterationCommandHandler(UserManager<User> userManager, IUploadService uploadService, D2DContext context, IAuthService authService)
        {
            _userManager = userManager;
            _uploadService = uploadService;
            _context = context;
            _authService = authService;
        }

        public async Task<JwtToken> Handle(DesignerRegesterationCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.DesignerId);
            if (user == null || !user.EmailConfirmed || user.UserType != UserType.Designer)
                throw new Exception("Invalid request");

            var stepUrls = new List<DesignVerification>();
            foreach (var file in request.StepUrls)
            {
                var url = await _uploadService.UploadFileAsync(file);
                stepUrls.Add(new DesignVerification { StepUrl = url });
            }
            Designer designer = (Designer) user;
            designer.FrontImageID = await _uploadService.UploadFileAsync(request.FrontImageID);
            designer.BackImageID = await _uploadService.UploadFileAsync(request.BackImageID);
            designer.PersonalImage = await _uploadService.UploadFileAsync(request.PersonalImage);
            designer.DesignVerifications = stepUrls;

            _context.Designers.Update(designer);
            await _context.SaveChangesAsync();
            //await _userManager.UpdateAsync(designer);

            var result = new Dictionary<string, string>
            {
                { "FrontImageID", designer.FrontImageID },
                { "BackImageID", designer.BackImageID },
                { "PersonalImage", designer.PersonalImage },
                { "DesignVerificationUrls", string.Join(", ", designer.DesignVerifications.Select(d => d.StepUrl)) }
            };
            var AccessToken = await _authService.GenerateAccessToken(designer);
            var refreshToken = await _authService.GenerateRefreshToken(designer.Id);
            return new JwtToken
            {
                UserID = designer.Id,
                AccessToken = AccessToken.Token,
                RefreshToken = refreshToken.Token,
                AccessTokenExpiresAt = AccessToken.ExpiresAt,
                RefreshTokenExpiresAt = refreshToken.ExpiresAt,
                Data = result,

            };
        }
    }
}
