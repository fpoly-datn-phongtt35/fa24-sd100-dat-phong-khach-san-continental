using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Policy
{
    public class PolicyTermsDto
    {
        public string PolicyTypeTitle { get; set; }
        public List<Guid> PolicyIds { get; set; }
        public List<string> PolicyTitles { get; set; }
        public List<string> PolicyContents { get; set; }
    }
}
