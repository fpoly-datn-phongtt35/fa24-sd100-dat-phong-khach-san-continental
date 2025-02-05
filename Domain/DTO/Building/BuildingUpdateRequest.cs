﻿using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO.Building
{
    public class BuildingUpdateRequest
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public EntityStatus? Status { get; set; } = EntityStatus.Active;
        public bool? Deleted { get; set; }
        public DateTimeOffset? ModifiedTime { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
