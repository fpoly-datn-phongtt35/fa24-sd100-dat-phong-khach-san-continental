using ViewClient.Models.DTO.Login;
using ViewClient.Models.DTO.Register;

namespace ViewClient.Repositories.IRepository
{
    public interface IRegister
    {
        Task<ClientAuthenicationViewModel> RegisterAsync(RegisterInputRequest request);
    }
}
