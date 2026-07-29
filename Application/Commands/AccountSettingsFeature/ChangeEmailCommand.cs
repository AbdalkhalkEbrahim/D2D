using Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.AccountSettingsFeature
{
    public class ChangeEmailCommand:IRequest<Result<string>>
    {
        public string Id { get; set; }
        public string Otp {  get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}
