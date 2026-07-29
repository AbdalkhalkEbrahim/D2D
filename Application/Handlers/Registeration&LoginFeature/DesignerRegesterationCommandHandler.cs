using Application.Commands.RegisterationFeature;
using Application.Interfaces;
using Application.Response;
using Domain.DTOs.RegisterationDtos;
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
      

        public DesignerRegesterationCommandHandler(UserManager<User> userManager, IUploadService uploadService, D2DContext context)
        {
            _userManager = userManager;
            _uploadService = uploadService;
            _context = context;
            
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