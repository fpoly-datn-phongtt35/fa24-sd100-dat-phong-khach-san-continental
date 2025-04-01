using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Helper
{
    public class PolicyTypeHelper
    {
        public static string DisplayPolicyType(PolicyTypeEnum type)
        {
            switch (type)
            {
                case PolicyTypeEnum.General:
                    return "Chính sách chung";
                case PolicyTypeEnum.PrivacyPolicy:
                    return "Chính sách bảo mật";
                case PolicyTypeEnum.RefundPolicy:
                    return "Chính sách hoàn trả";
                case PolicyTypeEnum.UsagePolicy:
                    return "Chính sách sử dụng";
                case PolicyTypeEnum.ServiceAgreement:
                    return "Thỏa thuận dịch vụ";
                case PolicyTypeEnum.PaymentPolicy:
                    return "Chính sách thanh toán";
                case PolicyTypeEnum.CancellationPolicy:
                    return "Chính sách hủy";
                case PolicyTypeEnum.Other:
                    return "Chính sách khác";
                case PolicyTypeEnum.BookingPolicy:
                    return "Chính sách đặt phòng";
                case PolicyTypeEnum.CheckInAndCheckOutPolicy:
                    return "Chính sách nhận phòng và trả phòng";
                default:
                    return "Unknown";
            }
        }
    }
}
