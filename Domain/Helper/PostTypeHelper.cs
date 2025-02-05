﻿using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Helper
{
    public class PostTypeHelper
    {
        public static string DisplayPostType(PostTypeEnum type)
        {
            switch (type)
            {
                case PostTypeEnum.General:
                    return "Chính sách chung";
                case PostTypeEnum.PrivacyPolicy:
                    return "Chính sách bảo mật";
                case PostTypeEnum.RefundPolicy:
                    return "Chính sách hoàn trả";
                case PostTypeEnum.UsagePolicy:
                    return "Chính sách sử dụng";
                case PostTypeEnum.ServiceAgreement:
                    return "Thỏa thuận dịch vụ";
                case PostTypeEnum.PaymentPolicy:
                    return "Chính sách thanh toán";
                case PostTypeEnum.CancellationPolicy:
                    return "Chính sách hủy";
                case PostTypeEnum.Other:
                    return "Chính sách khác";
                case PostTypeEnum.BookingPolicy:
                    return "Chính sách đặt phòng";
                case PostTypeEnum.CheckInAndCheckOutPolicy:
                    return "Chính sách nhận phòng và trả phòng";
                default:
                    return "Unknown";
            }
        }
    }
}
