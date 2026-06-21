using Application.Commands.AccountSettingsFeature;
using Application.Interfaces;
using Application.Response;
using Domain.DTOs.AccountSettingsDtos;
using Hangfire;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.AccountSettingsFeature
{
    public class EditProfileCommandHandler : IRequestHandler<EditProfileCommand, Result<ProfileResponse>>
    {
        private readonly D2DContext _context;
        private readonly IUploadService _uploadService;

        public EditProfileCommandHandler(D2DContext context, IUploadService uploadService)
        {
            _context = context;
            _uploadService = uploadService;
        }
        public async Task<Result<ProfileResponse>> Handle(EditProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);
            if (user == null)
                return Result<ProfileResponse>.Failure(Messages.NotFound.WithTarget("User"));

            if(request.FirstName != null)
                user.FirstName = request.FirstName;

            if(request.LastName != null)
                user.LastName = request.LastName;

            if(request.PhoneNumber != null)
                user.PhoneNumber = request.PhoneNumber;

            if(request.ProfileImageUrl != null)
            {
                var fileToBeUploaded = await _uploadService.ChangeFileFormat(new List<IFormFile> { request.ProfileImageUrl });

                BackgroundJob.Enqueue<IUploadService>(uploadService =>
                uploadService.UploadAndSaveSingleFile(user,"ProfileImageUrl", fileToBeUploaded[0], true));
            }

            _context.Update(user);
            await _context.SaveChangesAsync();

            return new ProfileResponse
            {
                AnonName = user.AnonName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                BD = user.BD
            };
        }
    }
}
