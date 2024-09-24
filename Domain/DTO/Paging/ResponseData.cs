using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Paging
{
    public class ResponseData<T>
    {
        public List<T> data {  get; set; }
        public int totalPage { get; set; }
        public int totalRecord {  get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
}
