using Domain.DTOs;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
    public class UserRegisterationCommand: IRequest<string>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ComfirmedPassword { get; set; }
        public UserType UserType { get; set; }
        public int Day { get;  set; }
        public int Month { get;  set; }
        public int Year { get;  set; }
    }
}
