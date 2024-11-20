using Domain.DTO.ServiceOrderDetail;

namespace ViewClient.Repositories.IRepository
{
    public interface IServiceOderDetail
    {
        Task<int> AddServiceOrderDetail(ServiceOrderDetailCreateRequest request);
    }
}
