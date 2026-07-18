using Application.Response;
using Domain.DTOs;
using Domain.DTOs.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IModelesService
    {
        public Task<Result<List<string>>> GenerateSummaries(SummaryGenerationPayload payload);
        public Task<Result<int>> CreateModelChate(string CustomerId);
        public Task<Result<decimal>> AnalaysisImageScore(string prompet, IFormFile image);
        public Task<Result> AnalysisUserDocuments(string userId, List<QwenImageItem> identityFiles, List<FileUploadModel> uploadedDocuments);
    }
}
