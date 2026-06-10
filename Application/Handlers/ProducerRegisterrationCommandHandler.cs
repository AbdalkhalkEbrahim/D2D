using Application.Commands;
using Application.Interfaces;
using Application.Response;
using Domain.DTOs;
using Domain.Entities.Customers;
using Domain.Entities.Producers;
using Domain.Entities.Shared;
using Domain.Enums;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Handlers
{
    public class ProducerRegisterrationCommandHandler : IRequestHandler<ProducerRegisterrationCommand, Result<ProducerRegisterationResponse>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUploadService _uploadService;
        private readonly D2DContext _context;
        private readonly IIdentityValidationService _identityValidationService;

        public ProducerRegisterrationCommandHandler(UserManager<User> userManager, IUploadService uploadService, IIdentityValidationService identityValidationService, D2DContext context)
        {
            _userManager = userManager;
            _uploadService = uploadService;
            _identityValidationService = identityValidationService;
            _context = context;
        }

        public async Task<Result<ProducerRegisterationResponse>> Handle(ProducerRegisterrationCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.ProducerId);
            if (user == null || !user.EmailConfirmed || user.UserType != UserType.Producer)
                return Result<ProducerRegisterationResponse>.Failure(Messages.BadRequest.WithTarget("InvalidRequest"));

            var licenseUrls = new List<LicenseVerification>();
            foreach (var file in request.LicenseUrls)
            {
                var url =  _uploadService.UploadFileAsync(file).ToString();
                if (url != null)
                    licenseUrls.Add(new LicenseVerification { LicenseUrl = url });
            }

            Producer producer = (Producer)user;
            var personalImageResult = await _uploadService.UploadFileAsync(request.PersonalImage);
            var frontImageResult = await _uploadService.UploadFileAsync(request.FrontImageID);
            var backImageResult = await _uploadService.UploadFileAsync(request.BackImageID);

            if (!personalImageResult.IsSuccess || !frontImageResult.IsSuccess || !backImageResult.IsSuccess)
                return Result<ProducerRegisterationResponse>.Failure(Messages.BadRequest.WithTarget("ImageUploadFailed"));

            producer.FrontImageID = personalImageResult.Value;
            producer.BackImageID = personalImageResult.Value;
            producer.PersonalImage = personalImageResult.Value;
            producer.LicenseVerifications = licenseUrls;

          
            _context.Producers.Update(producer);

            var result = new Dictionary<string, string>
            {
                { "FrontImageID", producer.FrontImageID },
                { "BackImageID", producer.BackImageID },
                { "PersonalImage", producer.PersonalImage },
                { "LicenseUrls", string.Join(", ", licenseUrls.Select(l => l.LicenseUrl)) }
            };

        checkAgain:
            var response = await _identityValidationService.AnalyzeAsync(result["FrontImageID"], result["BackImageID"], result["PersonalImage"]);
            if (response.Value.SimilarityScore is null)
                goto checkAgain;

            if (response.Value.SimilarityScore >= 0.8)
            {
                producer.IdentityStatus = VerificationStatus.Approved;
                _context.Producers.Update(producer);
            }
            await _context.SaveChangesAsync();

            return Result<ProducerRegisterationResponse>.Success(new ProducerRegisterationResponse
            {
                UserId = producer.Id,
                FrontImageID = producer.FrontImageID,
                BackImageID = producer.BackImageID,
                PersonalImage = producer.PersonalImage,
                VerificationStatus = producer.IdentityStatus,
                LicenseVerification=producer.LicenseVerifications,
                SimilarityScore = response.Value.SimilarityScore,
                DocumentQuality = response.Value.DocumentQuality,
                NeedsManualReview = response.Value.NeedsManualReview,
                Notes = response.Value.Notes,
            });
        }
    }
}