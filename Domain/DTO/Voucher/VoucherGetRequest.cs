using Domain.DTO.Paging;
using Domain.Enums;

namespace Domain.DTO.Voucher
{
	public class VoucherGetRequest : PagingRequest
	{
		public DiscountType DiscountType { get; set; }
	}
}
