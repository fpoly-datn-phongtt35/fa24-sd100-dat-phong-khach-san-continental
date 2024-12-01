using Domain.Models;

namespace ViewClient.Repositories.IRepository
{
    public interface IServiceOderDetail
    {
        Task<int> AddServiceOrderDetail(ServiceOrderDetail request);
    }
}
