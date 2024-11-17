using Domain.Enums;

namespace Domain.DTO.Voucher
{
	public class VoucherCreateRequest
	{
		public string? Name { get; set; }
		public string? Description { get; set; }
		public DiscountType? DiscountType { get; set; }
		public decimal? DiscountValue { get; set; }
		public EntityStatus? Status { get; set; } = EntityStatus.Active;

		public DateTimeOffset? CreatedTime { get; set; }
		public Guid? CreatedBy { get; set; }
	}
}
