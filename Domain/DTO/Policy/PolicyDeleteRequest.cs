namespace Domain.DTO.Policy
{
    public class PolicyDeleteRequest
    {
        public Guid Id { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
    }
}
