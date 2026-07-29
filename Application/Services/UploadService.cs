using Application.Extensions;
using Application.Interfaces;
using Application.Response;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.DTOs;
using Domain.DTOs.Model;
using Domain.Entities.Designers;
using Domain.Entities.Producers;
using Domain.Enums.Status;
using Domain.Enums.Types;
using Domain.Settings;
using Hangfire;
using Infrastructure.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using static System.Net.Mime.MediaTypeNames;

namespace Application.Services
{
    public class UploadService : IUploadService
    {
        private readonly Cloudinary _cloudinary;
        private readonly D2DContext _context;
        public UploadService(IOptions<CloudinarySettings> config, D2DContext context)
        {
            var account = new Account(
               config.Value.CloudName,
               config.Value.ApiKey,
               config.Value.ApiSecret
           );

            _cloudinary = new Cloudinary(account);
            _context = context;
        }
        public async Task<Result<List<string>>> UploadFileAsync(List<FileUploadModel> files)
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

        public async Task<Result> UploadAndSaveSingleFile<T>(T obj, string property, FileUploadModel file, bool updateOrAdd)
        {
            if (file == null)
                return Result.Failure(Messages.BadRequest.WithTarget("NullValue"));

            using (var stream = new MemoryStream(file.FileBytes))
            {

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                    throw new Exception($"Cloudinary Error: {uploadResult.Error.Message}");

                var secureUrl = uploadResult.SecureUrl.ToString();

                obj.Set(property, secureUrl);

            }
            if (updateOrAdd)
                _context.Update(obj);
            else
                _context.Add(obj);

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

        public async Task<FileUploadModel> ChangeFileFormat(IFormFile singlefile)
        {
            using var ms = new MemoryStream();

            await singlefile.CopyToAsync(ms);

            return new FileUploadModel
            {
                FileBytes = ms.ToArray(),
                FileName = singlefile.FileName
            };
        }

        public async Task<List<QwenImageItem>> ChangeFileFormateToBase64(List<IFormFile> files)
        {
            var images = new List<QwenImageItem>();

            foreach (var file in files)
            {
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);

                images.Add(new QwenImageItem
                {
                    DataBase64 = Convert.ToBase64String(ms.ToArray()),
                    Type = file.ContentType
                });
            }
            return images;
        }
    }
}