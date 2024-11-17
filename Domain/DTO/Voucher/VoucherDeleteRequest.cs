namespace Domain.DTO.Voucher
{
	public class VoucherDeleteRequest
	{
		public Guid Id { get; set; }
		public Guid? DeletedBy { get; set; }
		public DateTimeOffset? DeletedTime { get; set; }
	}
}
