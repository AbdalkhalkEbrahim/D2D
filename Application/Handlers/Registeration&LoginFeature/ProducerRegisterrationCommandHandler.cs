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
using Microsoft.EntityFrameworkCore;

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
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u=>u.Id == request.ProducerId);
            if (user == null || !user.EmailConfirmed || user.UserType != UserType.Producer)
                return Result<ProducerRegisterationResponse>.Failure(Messages.BadRequest.WithTarget("InvalidRequest"));

            List<IFormFile> files = request.LicenseUrls;
            files.AddRange(new List<IFormFile> { request.FrontImageID, request.BackImageID, request.PersonalImage });

            var filesToBeUploaded = await _uploadService.ChangeFileFormat(files);

            List<string> links = new List<string>();
            BackgroundJob.Enqueue<IUploadService>(uploadService =>
                uploadService.UploadAndSaveUserDocsAsync(user.Id,user.UserType,filesToBeUploaded)
                );

            user.IdentityStatus = VerificationStatus.Approved;
            return Result<ProducerRegisterationResponse>.Success(new ProducerRegisterationResponse
            {
                UserId = user.Id,
                VerificationStatus = user.IdentityStatus,

/*                SimilarityScore = response.Value.SimilarityScore,
                DocumentQuality = response.Value.DocumentQuality,
               Notes = response.Value.Notes*/
            });
        }
    }
}