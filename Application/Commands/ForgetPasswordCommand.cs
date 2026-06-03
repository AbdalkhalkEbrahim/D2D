using MediatR;

namespace Application.Commands
{
    public class ForgetPasswordCommand:IRequest<string>
    {
        public string Email { get; set; }
        public string Otp { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
