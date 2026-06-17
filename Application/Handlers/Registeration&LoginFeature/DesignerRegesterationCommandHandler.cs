using Application.Commands.RegisterationFeature;
using Application.Interfaces;
using Application.Response;
using CloudinaryDotNet.Actions;
using Domain.DTOs.ModelDtos;
using Domain.DTOs.RegisterationDtos;
using Domain.Entities.Designers;
using Domain.Entities.Shared;
using Domain.Enums.Status;
using Domain.Enums.Types;
using Hangfire;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Handlers
{
    public class DesignerRegesterationCommandHandler : IRequestHandler<DesignerRegesterationCommand, Result<DesignerRegisterationResponse>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUploadService _uploadService;
        private readonly D2DContext _context;
        private readonly IIdentityValidationService _identityValidationService;
        private readonly IDesignValidationService _designValidationService;

        public DesignerRegesterationCommandHandler(UserManager<User> userManager, IUploadService uploadService, D2DContext context, IIdentityValidationService identityValidationService, IDesignValidationService designValidationService)
        {
            _userManager = userManager;
            _uploadService = uploadService;
            _context = context;
            _identityValidationService = identityValidationService;
            _designValidationService = designValidationService;
        }

        public async Task<Result<DesignerRegisterationResponse>> Handle(DesignerRegesterationCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.DesignerId);
            if (user == null || !user.EmailConfirmed || user.UserType != UserType.Designer)
                return Result<DesignerRegisterationResponse>.Failure(Messages.BadRequest.WithTarget("InvalidRequest"));

            List<IFormFile> files = request.StepUrls;
            files.AddRange(new List<IFormFile> { request.FrontImageID, request.BackImageID, request.PersonalImage });

            var filesToBeUploaded = await _uploadService.ChangeFileFormat(files);

            List<string> links = new List<string>();
            BackgroundJob.Enqueue<IUploadService>(uploadService =>
                uploadService.UploadAndSaveUserDocsAsync(user.Id, user.UserType, filesToBeUploaded)
                );

            user.IdentityStatus = VerificationStatus.Approved;
            return Result<DesignerRegisterationResponse>.Success(new DesignerRegisterationResponse
            {
                UserId = user.Id,
                VerificationStatus = user.IdentityStatus,
/*                SimilarityScore = identityResponse.Value.SimilarityScore,
                DocumentQuality = identityResponse.Value.DocumentQuality,
                NeedsManualReview = identityResponse.Value.NeedsManualReview,
                Notes = identityResponse.Value.Notes,*/
            });
        }

       
    }
}