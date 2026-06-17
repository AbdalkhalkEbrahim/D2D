using Application.Commands.RegisterationFeature;
using Application.Interfaces;
using Application.Response;
using Domain.DTOs.RegisterationDtos;
using Domain.Entities.Customers;
using Domain.Entities.Shared;
using Domain.Enums.Status;
using Domain.Enums.Types;
using Hangfire;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;


namespace Application.Handlers
{
    public class CustomerRegisterationCommandHandler : IRequestHandler<CustomerRegisterationCommand, Result<CustomerRegisteratonResponse>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUploadService _uploadService;
        private readonly IIdentityValidationService _identityValidationService;
        private readonly D2DContext _context;

        public CustomerRegisterationCommandHandler(UserManager<User> userManager, IUploadService uploadService, IIdentityValidationService identityValidationService, D2DContext context)
        {
            _userManager = userManager;
            _uploadService = uploadService;
            _identityValidationService = identityValidationService;
            _context = context;
        }

        public async Task<Result<CustomerRegisteratonResponse>> Handle(CustomerRegisterationCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u=>u.Id == request.CustomerId);
            if (user == null || !user.EmailConfirmed || user.UserType != UserType.Customer)
                return Result<CustomerRegisteratonResponse>.Failure(Messages.BadRequest.WithTarget("InvalidRequest"));

            Customer customer = (Customer)user;
            customer.Addresses = new HashSet<Address>
            {
                new Address
                {
                    AppartmentNo = request.AppartmentNo,
                    BuildingNumber = request.BuildingNumber,
                    Street = request.Street,
                    District = request.District,
                    City = request.City,
                    Goverate = request.Goverate,
                    Selected = true,
                }
            };

            var filesToBeUploaded = await _uploadService.ChangeFileFormat(new List<IFormFile> { request.FrontImageID, request.BackImageID, request.PersonalImage });

            BackgroundJob.Enqueue<IUploadService>(uploadService =>
                uploadService.UploadAndSaveUserDocsAsync(user.Id,user.UserType,filesToBeUploaded)
                );
            user.IdentityStatus = VerificationStatus.Approved;
            return Result<CustomerRegisteratonResponse>.Success(new CustomerRegisteratonResponse
            {
                UserId = customer.Id,
                VerificationStatus=customer.IdentityStatus,
                //SimilarityScore=response.Value.SimilarityScore,
                //DocumentQuality=response.Value.DocumentQuality,
                //NeedsManualReview=response.Value.NeedsManualReview,
                //Notes=response.Value.Notes,
            });
        }
    }
}