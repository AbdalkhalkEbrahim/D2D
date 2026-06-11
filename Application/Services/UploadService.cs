using Application.Interfaces;
using Application.Response;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Application.Services
{
    public class UploadService : IUploadService
    {
        private readonly Cloudinary _cloudinary;
        public UploadService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
               config.Value.CloudName,
               config.Value.ApiKey,
               config.Value.ApiSecret
           );

            _cloudinary = new Cloudinary(account);
        }
        public async Task<Result<string>> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Result<string>.Failure(Messages.NotFound.WithTarget("File"));

            var uploadResult = new ImageUploadResult();

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            if (uploadResult.Error != null)
                return Result<string>.Failure(new Application.Response.Error("Cloudinary Error", uploadResult.Error.Message));


            return Result<string>.Success(uploadResult.SecureUrl.ToString());
        }
    }
}