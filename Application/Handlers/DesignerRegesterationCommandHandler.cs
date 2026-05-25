using Application.Commands;
using Domain.Entities.Designers;
using Domain.Entities.Producers;
using Domain.Entities.Shared;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class DesignerRegesterationCommandHandler : IRequestHandler<DesignerRegesterationCommand, Dictionary<string, string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUploadService _uploadService;
        public DesignerRegesterationCommandHandler(UserManager<User> userManager, IUploadService uploadService)
        {
            _userManager = userManager;
            _uploadService = uploadService;
        }

        public async Task<Dictionary<string, string>> Handle(DesignerRegesterationCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.DesignerId);
            if (user == null || !user.EmailConfirmed || user.UserType != UserType.Designer)
                throw new Exception("Invalid request");

            var stepUrls = new List<DesignVerification>();
            request.StepUrls.ForEach(async file =>
            {
                var url = await _uploadService.UploadFileAsync(file);
                stepUrls.Add(new DesignVerification { StepUrl = url });
            });
            var designer = new Designer
            {
                Id = user.Id,
                FrontImageID = await _uploadService.UploadFileAsync(request.FrontImageID),
                BackImageID = await _uploadService.UploadFileAsync(request.BackImageID),
                PersonalImage = await _uploadService.UploadFileAsync(request.PersonalImage),
                DesignVerifications = stepUrls
            };
            await _userManager.UpdateAsync(designer);
            var result = new Dictionary<string, string>
            {
                { "FrontImageID", designer.FrontImageID },
                { "BackImageID", designer.BackImageID },
                { "PersonalImage", designer.PersonalImage },
                { "DesignVerificationUrls", string.Join(", ", designer.DesignVerifications.Select(d => d.StepUrl)) }
            };
            return result;
        }
    }
}
