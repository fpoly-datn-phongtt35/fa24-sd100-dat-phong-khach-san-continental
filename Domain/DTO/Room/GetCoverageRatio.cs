using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Room
{
    public class MonthlyCoverageDto
    {
        public int YearNumber { get; set; }
        public int MonthNumber { get; set; }
        public double CoverageRatio { get; set; }
    }

    public class WeeklyCoverageDto
    {
        public DateTime Date { get; set; }
        public double CoverageRatio { get; set; }
    }


}
