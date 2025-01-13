using Domain.DTO.Email;

namespace ViewClient.Repositories.IRepository
{
    public interface ISendEmail
    {
        Task<int> SendAccountAsync(AccountRequest request);
        Task<int> SendEmailAsync(EmailRequest request);
    }
}
