using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum PostTypeEnum
    {
        General = 0,            
        PrivacyPolicy = 1,    
        RefundPolicy = 2,      
        UsagePolicy = 3,        
        ServiceAgreement = 4, 
        PaymentPolicy = 5,     
        CancellationPolicy = 6, 
        BookingPolicy = 7,
        CheckInAndCheckOutPolicy = 8,
        Other = 99
    }
}
