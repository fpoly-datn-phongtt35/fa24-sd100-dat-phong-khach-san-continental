using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Post
{
    public class PostTermsDto
    {
        public string PostTypeTitle { get; set; }
        public List<Guid> PostIds { get; set; }
        public List<string> PostTitles { get; set; }
        public List<string> PostContents { get; set; }
    }
}
