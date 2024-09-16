﻿using Domain.DTO.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Building
{
    public class BuildingGetRequest: PagingRequest
    {
        public string Name { get; set; } = string.Empty;
    }
}
