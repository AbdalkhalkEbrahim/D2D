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
    public class DesignerRegesterationCommandHandler : IRequestHandler<DesignerRegesterationCommand, object>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUploadService _uploadService;
        private readonly D2DContext _context;
        private readonly IAuthService _authService;
        private readonly IIdentityValidationService _identityValidationService;
        private readonly IDesignValidationService _designValidationService;
        public DesignerRegesterationCommandHandler(UserManager<User> userManager, IUploadService uploadService, D2DContext context, IAuthService authService, IIdentityValidationService identityValidationService, IDesignValidationService designValidationService)
        {
            _userManager = userManager;
            _uploadService = uploadService;
            _context = context;
            _authService = authService;
            _identityValidationService = identityValidationService;
            _designValidationService = designValidationService;
        }

        public async Task<object> Handle(DesignerRegesterationCommand request, CancellationToken cancellationToken)
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
            //await _context.SaveChangesAsync();
            //await _userManager.UpdateAsync(designer);

            var result = new Dictionary<string, string>
            {
                { "FrontImageID", designer.FrontImageID },
                { "BackImageID", designer.BackImageID },
                { "PersonalImage", designer.PersonalImage },
              //  { "DesignVerificationUrls", string.Join(", ", designer.DesignVerifications.Select(d => d.StepUrl)) }
            };

        checkIdentityAgain:
            var identityResponse = await _identityValidationService.AnalyzeAsync(result["FrontImageID"], result["BackImageID"], result["PersonalImage"]);
            if (identityResponse.SimilarityScore is null)
                goto checkIdentityAgain;

            if (identityResponse.SimilarityScore >= 0.8)
            {
                designer.IdentityStatus = VerificationStatus.Approved;
                _context.Designers.Update(designer);
            }
            CheckDesignAgain:
            var designResponse = await _designValidationService.AnalyzeAsync(designer.DesignVerifications.Select(d => d.StepUrl).ToList());
            if(designResponse.ConfidenceScore is null || designResponse.ProgressScore is null)
                goto CheckDesignAgain;

            await _context.SaveChangesAsync();

            return new
            {
                IdentityResponse = identityResponse,
                DesignResponse = designResponse,
            };

            /*  var AccessToken = await _authService.GenerateAccessToken(designer);
              var refreshToken = await _authService.GenerateRefreshToken(designer.Id);
              return new JwtToken
              {
                  UserID = designer.Id,
                  AccessToken = AccessToken.Token,
                  RefreshToken = refreshToken.Token,
                  AccessTokenExpiresAt = AccessToken.ExpiresAt,
                  RefreshTokenExpiresAt = refreshToken.ExpiresAt,
                  Data = result,

              };*/
        }
    }
}
