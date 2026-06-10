using Application.Commands;
using Application.Interfaces;
using Application.Response;
using Domain.DTOs;
using Domain.Entities.Customers;
using Domain.Entities.Shared;
using Domain.Enums;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;


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
            var user = await _userManager.FindByIdAsync(request.CustomerId);
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
            var personalImageResult = await _uploadService.UploadFileAsync(request.PersonalImage);
            var frontImageResult = await _uploadService.UploadFileAsync(request.FrontImageID);
            var backImageResult = await _uploadService.UploadFileAsync(request.BackImageID);

            if (!personalImageResult.IsSuccess || !frontImageResult.IsSuccess || !backImageResult.IsSuccess)
                return Result<CustomerRegisteratonResponse>.Failure(Messages.BadRequest.WithTarget("ImageUploadFailed"));

            customer.PersonalImage = personalImageResult.Value;
            customer.FrontImageID = frontImageResult.Value;
            customer.BackImageID = backImageResult.Value;
            _context.Customers.Update(customer);
 
            var result = new Dictionary<string, string>
            {
                { "FrontImageID", customer.FrontImageID },
                { "BackImageID", customer.BackImageID },
                { "PersonalImage", customer.PersonalImage }
            };

        checkAgain:
            var response = await _identityValidationService.AnalyzeAsync(result["FrontImageID"], result["BackImageID"], result["PersonalImage"]);
            
            if (response.Value.SimilarityScore is null)
                goto checkAgain;

            if (response.Value.SimilarityScore >= 0.8)
            {
                customer.IdentityStatus = VerificationStatus.Approved;
                _context.Customers.Update(customer);
            }
            await _context.SaveChangesAsync();

            return Result<CustomerRegisteratonResponse>.Success(new CustomerRegisteratonResponse
            {
                UserId = customer.Id,
                FrontImageID=customer.FrontImageID,
                BackImageID=customer.BackImageID,
                PersonalImage = customer.PersonalImage,
                VerificationStatus=customer.IdentityStatus,
                SimilarityScore=response.Value.SimilarityScore,
                DocumentQuality=response.Value.DocumentQuality,
                NeedsManualReview=response.Value.NeedsManualReview,
                Notes=response.Value.Notes,
            });
        }
    }
}