using Domain.Enums;

namespace ViewClient.Models.DTO.Login
{
    public class ClientAuthenicationViewModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public EntityStatus Status { get; set; }
    }
}
