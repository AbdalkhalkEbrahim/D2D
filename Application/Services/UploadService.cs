using Application.Interfaces;
using Application.Response;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.DTOs;
using Domain.Entities.Designers;
using Domain.Entities.Producers;
using Domain.Enums.Types;
using Domain.Settings;
using Infrastructure.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Application.Services
{
    public class UploadService : IUploadService
    {
        private readonly Cloudinary _cloudinary;
        private readonly D2DContext _context;
        public UploadService(IOptions<CloudinarySettings> config,D2DContext context)
        {
            var account = new Account(
               config.Value.CloudName,
               config.Value.ApiKey,
               config.Value.ApiSecret
           );

            _cloudinary = new Cloudinary(account);
            _context = context;
        }
/*        public async Task<Result<List<string>>> UploadFileAsync(List<FileUploadModel> files)
        {
            List<string> links = new List<string>();
            if (files == null || files.Count == 0)
                return Result<List<string>>.Failure(Messages.BadRequest.WithTarget("NullValue"));

            foreach (var file in files)
            {
                using (var stream = new MemoryStream(file.FileBytes))
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                    if (uploadResult.Error != null)
                        throw new Exception($"Cloudinary Error for {file.FileName}: {uploadResult.Error.Message}");

                    links.Add(uploadResult.SecureUrl.ToString());
                }
            }
            return Result<List<string>>.Success(links);
        }
*/
        
        public async Task<Result> UploadAndSaveUserDocsAsync(string userId, UserType userType, List<FileUploadModel> files)
        {
            if (files == null || files.Count == 0)
                return Result.Failure(Messages.BadRequest.WithTarget("NullValue"));

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return Result.Failure(Messages.NotFound.WithTarget("User"));
            Producer producer = new Producer();
            Designer designer = new Designer();

            if (userType == UserType.Producer)
                producer = (Producer)user;
            else if (userType == UserType.Designer)
                designer = (Designer)user;

            for (int i = 0; i < files.Count; i++)
            {
                using (var stream = new MemoryStream(files[i].FileBytes))
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(files[i].FileName, stream),
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                    if (uploadResult.Error != null)
                        throw new Exception($"Cloudinary Error: {uploadResult.Error.Message}");

                    var secureUrl = uploadResult.SecureUrl.ToString();

                    switch (i)
                    {
                        case 0:
                            user.FrontImageID = secureUrl;
                            break;
                        case 1:
                            user.BackImageID = secureUrl;
                            break;
                        case 2:
                            user.PersonalImage = secureUrl;
                            break;
                        default:
                            switch (userType) {
                                case UserType.Producer:
                                    producer.LicenseVerifications.Add(new LicenseVerification{LicenseUrl = secureUrl });
                                break;
                                case UserType.Designer:
                                    designer.DesignVerifications.Add(new DesignVerification { StepUrl = secureUrl });
                                break;
                            }
                        break;
                    }
                }
            }
            _context.Update(userType == UserType.Customer ? user : (userType == UserType.Producer ? producer : designer));
            await _context.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<List<FileUploadModel>> ChangeFileFormat(List<IFormFile> files)
        {
            var filesToUpload = new List<FileUploadModel>();

            foreach (var file in files)
            {
                if (file != null && file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await file.CopyToAsync(ms);
                        filesToUpload.Add(new FileUploadModel
                        {
                            FileBytes = ms.ToArray(),
                            FileName = file.FileName
                        });
                    }
                }
            }
            return filesToUpload;
        }
    }
}