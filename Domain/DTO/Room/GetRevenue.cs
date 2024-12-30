using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Room
{
    public class GetRevenue
    {
            public string Period { get; set; } // Tháng hoặc Năm
            public decimal TotalAmount { get; set; } // Tổng doanh thu
        

    }
}
