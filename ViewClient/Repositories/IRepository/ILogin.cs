using ViewClient.Models.DTO.Login;

namespace ViewClient.Repositories.IRepository
{
    public interface ILogin
    {
        Task<ClientAuthenicationViewModel> LoginAsync(LoginInputRequest request);
    }
}
