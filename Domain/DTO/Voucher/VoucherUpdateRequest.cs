using Domain.Enums;

namespace Domain.DTO.Voucher
{
	public class VoucherUpdateRequest
	{
		public Guid Id { get; set; }
		public string? Name { get; set; }
		public string? Description { get; set; }
		public DiscountType? DiscountType { get; set; }
		public decimal? DiscountValue { get; set; }
		public EntityStatus? Status { get; set; } = EntityStatus.Active;
        public bool? Deleted { get; set; }

        public DateTimeOffset? ModifiedTime { get; set; }
		public Guid? ModifiedBy { get; set; }
	}
}
