using Application.Commands.AccountSettingsFeature;
using Application.Interfaces;
using Application.Response;
using Domain.DTOs.AccountSettingsDtos;
using Domain.Entities.Customers;
using Domain.Entities.Designs;
using Domain.Entities.Offers;
using Domain.Entities.Shared;
using Hangfire;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using jsonPatch = Microsoft.AspNetCore.JsonPatch.Operations;


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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId&& !u.IsDeleted);
            if (user == null)
                return Result<ProfileResponse>.Failure(Messages.NotFound.WithTarget("User"));

            //if(request.ProfileImageUrl != null)
            //{
            //    var fileToBeUploaded = await _uploadService.ChangeFileFormat(new List<IFormFile> { request.ProfileImageUrl });

            //    BackgroundJob.Enqueue<IUploadService>(uploadService =>
            //    uploadService.UploadAndSaveSingleFile(user,"ProfileImageUrl", fileToBeUploaded[0], true));
            //}

            var entityPatch = new JsonPatchDocument<User>();
            request.data.Operations.ForEach(op => entityPatch.Operations.Add(new jsonPatch.Operation<User>(op.op, op.path, op.from, op.value)));
            // Console.WriteLine(request.data.Operations.Count);
            entityPatch.ApplyTo(user);

            //_context.Update(user);
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
