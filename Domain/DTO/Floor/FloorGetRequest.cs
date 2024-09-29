﻿using Domain.DTO.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Floor
{
    public class FloorGetRequest : PagingRequest
    {
        public string Name { get; set; } = string.Empty;
    }
}
