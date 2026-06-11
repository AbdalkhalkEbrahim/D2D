using Application.Response;


namespace Application.Interfaces
{
    public interface IEmailService
    {
        Task<Result> SendEmailAsync(string toEmail, string subject, string body);
    }
}
