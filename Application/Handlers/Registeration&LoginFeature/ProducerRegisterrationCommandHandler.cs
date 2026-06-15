using Application.Commands.RegisterationFeature;
using Application.Interfaces;
using Application.Response;
using Domain.DTOs.RegisterationDtos;
using Domain.Entities.Producers;
using Domain.Entities.Shared;
using Domain.Enums.Status;
using Domain.Enums.Types;
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
            // 1. Initial Validation (Fail fast)
            var user = await _userManager.FindByIdAsync(request.ProducerId);
            if (user == null || !user.EmailConfirmed || user.UserType != UserType.Producer)
                return Result<ProducerRegisterationResponse>.Failure(Messages.BadRequest.WithTarget("InvalidRequest"));

            // 2. Start single uploads SIMULTANEOUSLY (Notice: NO 'await' here)
            Task<Result<string>> personalImageTask = _uploadService.UploadFileAsync(request.PersonalImage);
            Task<Result<string>> frontImageTask = _uploadService.UploadFileAsync(request.FrontImageID);
            Task<Result<string>> backImageTask = _uploadService.UploadFileAsync(request.BackImageID);

            // 3. Start the list of license uploads SIMULTANEOUSLY using LINQ
            List<Task<Result<string>>> licenseTasks = request.LicenseUrls
                .Select(file => _uploadService.UploadFileAsync(file))
                .ToList();

            // 4. Group all tasks together into one big execution pool
            var allUploadTasks = new List<Task>();
            allUploadTasks.Add(personalImageTask);
            allUploadTasks.Add(frontImageTask);
            allUploadTasks.Add(backImageTask);
            allUploadTasks.AddRange(licenseTasks); // Add the list of license tasks

            // 5. CRITICAL STEP: Await them all at the exact same time
            // This tells .NET to fire them over the network concurrently
            await Task.WhenAll(allUploadTasks);

            // 6. Now that Task.WhenAll is done, extracting '.Result' or 'awaiting' them is instant 
            var personalResult = await personalImageTask;
            var frontResult = await frontImageTask;
            var backResult = await backImageTask;

            if (!personalResult.IsSuccess || !frontResult.IsSuccess || !backResult.IsSuccess)
                return Result<ProducerRegisterationResponse>.Failure(Messages.BadRequest.WithTarget("ImageUploadFailed"));

            // 7. Process the license results securely
            var licenseUrls = new List<LicenseVerification>();
            foreach (var task in licenseTasks)
            {
                var licenseResult = await task; // Instant because it already finished in step 5
                if (licenseResult.IsSuccess && licenseResult.Value != null)
                {
                    licenseUrls.Add(new LicenseVerification { LicenseUrl = licenseResult.Value });
                }
            }
            
/*            var response = await _identityValidationService.AnalyzeAsync(frontResult.Value, backResult.Value, personalResult.Value);
            if(!response.IsSuccess)
                return Result<ProducerRegisterationResponse>.Failure(new Error("None",response.Error.Message));*/
            // 8. Bind data to your entity (Fixed your previous copy-paste variable bugs)
            Producer producer = (Producer)user;
            producer.PersonalImage = personalResult.Value;
            producer.FrontImageID = frontResult.Value;
            producer.BackImageID = backResult.Value;
            producer.LicenseVerifications = licenseUrls;

            _context.Producers.Update(producer);
            await _context.SaveChangesAsync();

            return Result<ProducerRegisterationResponse>.Success(new ProducerRegisterationResponse
            {
                UserId = producer.Id,
                FrontImageID = producer.FrontImageID,
                BackImageID = producer.BackImageID,
                PersonalImage = producer.PersonalImage,
                VerificationStatus = producer.IdentityStatus,
                LicenseVerification = string.Join(", ", licenseUrls.Select(l => l.LicenseUrl)),
/*                SimilarityScore = response.Value.SimilarityScore,
                DocumentQuality = response.Value.DocumentQuality,
               Notes = response.Value.Notes*/
            });
        }
    }
}