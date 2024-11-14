using ViewClient.Models.DTO.Login;

namespace ViewClient.Repositories.IRepository
{
    public interface ILogin
    {
        Task<ViewLoginInput> Login(LoginInputRequest request);
    }
}
