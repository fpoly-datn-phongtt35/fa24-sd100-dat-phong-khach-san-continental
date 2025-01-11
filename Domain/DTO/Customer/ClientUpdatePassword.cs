namespace Domain.DTO.Customer
{
    public class ClientUpdatePassword
    {
        public Guid Id { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
    }
}
