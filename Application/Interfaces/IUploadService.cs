using Application.Response;
using Domain.DTOs;
using Domain.Enums.Types;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IUploadService
    {
       // public Task<Result<List<string>>> UploadFileAsync(List<FileUploadModel> files);
        public Task<List<FileUploadModel>> ChangeFileFormat(List<IFormFile> files);
        public Task<Result> UploadAndSaveUserDocsAsync(string userId, UserType userType, List<FileUploadModel> files);

    }
}
