using Application.Commands.RegisterationFeature;
using Application.Interfaces;
using Application.Response;
using Domain.DTOs.RegisterationDtos;
using Domain.Entities.Producers;
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
        

        public ProducerRegisterrationCommandHandler(UserManager<User> userManager, IUploadService uploadService,  D2DContext context)
        {
            _userManager = userManager;
            _uploadService = uploadService;
            _context = context;
        }

        public async Task<Result<ProducerRegisterationResponse>> Handle(ProducerRegisterrationCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u=>u.Id == request.ProducerId);
            if (user == null || !user.EmailConfirmed || user.UserType != UserType.Producer)
                return Result<ProducerRegisterationResponse>.Failure(Messages.BadRequest.WithTarget("InvalidRequest"));


            var filesBase64 = await _uploadService.ChangeFileFormateToBase64(request.IdentityFiles);
            var files = await _uploadService.ChangeFileFormat(request.IdentityFiles);
            BackgroundJob.Enqueue<IModelesService>(uploadService =>
                uploadService.AnalysisUserDocuments(user.Id, filesBase64, files)
                );

            var licenseFormat = await _uploadService.ChangeFileFormat(request.LicenseUrls);
            foreach (var file in licenseFormat)
            {
                var lic = new LicenseVerification
                {
                    ProducerID = user.Id
                };

                BackgroundJob.Enqueue<IUploadService>(uploadService =>
               uploadService.UploadAndSaveSingleFile(lic, "LicenseUrl",file,false)
               );
            }
           

            return Result<ProducerRegisterationResponse>.Success(new ProducerRegisterationResponse
            {
                UserId = user.Id,
                VerificationStatus = user.IdentityStatus,
            });
        }
    }
}