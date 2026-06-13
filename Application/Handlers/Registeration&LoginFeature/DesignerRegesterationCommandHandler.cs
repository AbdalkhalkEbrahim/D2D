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
using Infrastructure.Data.Context;
using MediatR;
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

            var stepUrls = new List<DesignVerification>();

            foreach (var file in request.StepUrls)
            {
                var url =await  _uploadService.UploadFileAsync(file);
                if (url.IsSuccess )
                 stepUrls.Add(new DesignVerification { StepUrl = url.Value });
                else
                    return  Result<DesignerRegisterationResponse>.Failure(Messages.CloudinaryError(url.Error.Message));
            }

            Designer designer = (Designer)user;

            var personalImageResult = await _uploadService.UploadFileAsync(request.PersonalImage);
            var frontImageResult = await _uploadService.UploadFileAsync(request.FrontImageID);
            var backImageResult = await _uploadService.UploadFileAsync(request.BackImageID);

            if (!personalImageResult.IsSuccess || !frontImageResult.IsSuccess || !backImageResult.IsSuccess)
                return Result<DesignerRegisterationResponse>.Failure(Messages.BadRequest.WithTarget("ImageUploadFailed"));

            designer.FrontImageID = personalImageResult.Value;
            designer.BackImageID = personalImageResult.Value;
            designer.PersonalImage = personalImageResult.Value;
            designer.DesignVerifications = stepUrls;

           
            _context.Designers.Update(designer);

            var result = new Dictionary<string, string>
            {
                { "FrontImageID", designer.FrontImageID },
                { "BackImageID", designer.BackImageID },
                { "PersonalImage", designer.PersonalImage },
                { "StepUrls", string.Join(", ", stepUrls.Select(s => s.StepUrl)) }
            };

        checkIdentityAgain:
            var identityResponse = await _identityValidationService.AnalyzeAsync(result["FrontImageID"], result["BackImageID"], result["PersonalImage"]);
            if (identityResponse.Value.SimilarityScore is null)
                goto checkIdentityAgain;

            if (identityResponse.Value.SimilarityScore >= 0.8)
            {
                designer.IdentityStatus = VerificationStatus.Approved;
                _context.Designers.Update(designer);
            }

        //CheckDesignAgain:
        //    var designResponse = await _designValidationService.AnalyzeAsync(designer.DesignVerifications.Select(d => d.StepUrl).ToList());
        //    if (!designResponse.IsSuccess)
        //        return Result<DesignerRegisterationResponse>.Failure(new Error("SystemError",designResponse.Error.Message));

        //    if (designResponse.Value.ConfidenceScore is null || designResponse.Value.ProgressScore is null)
        //        goto CheckDesignAgain;

            await _context.SaveChangesAsync();

            return Result<DesignerRegisterationResponse>.Success(new DesignerRegisterationResponse
            {
                UserId = designer.Id,
                FrontImageID = designer.FrontImageID,
                BackImageID = designer.BackImageID,
                PersonalImage = designer.PersonalImage,
                DesignVerification = result["StepUrls"],
                VerificationStatus = designer.IdentityStatus,
                SimilarityScore = identityResponse.Value.SimilarityScore,
                DocumentQuality = identityResponse.Value.DocumentQuality,
                NeedsManualReview = identityResponse.Value.NeedsManualReview,
                Notes = identityResponse.Value.Notes,
            });
        }

       
    }
}