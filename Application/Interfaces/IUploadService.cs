using Application.Response;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IUploadService
    {
        Task<Result<string>> UploadFileAsync(IFormFile file);
    }
}
