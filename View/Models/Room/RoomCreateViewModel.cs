using Domain.Enums;

namespace View.Models.Room
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;

    public class RoomCreateViewModel
    {
        [Required(ErrorMessage = "Tên phòng không được để trống")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Giá phòng không được để trống")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phòng phải lớn hơn hoặc bằng 0")]
        public decimal? Price { get; set; }

        public string Address { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;
        [Range(0.1, double.MaxValue, ErrorMessage = "Diện tích phòng phải lớn hơn 0")]
        public double? RoomSize { get; set; }

        public Guid? FloorId { get; set; }

        public Guid RoomTypeId { get; set; }

        public RoomStatus Status { get; set; } = RoomStatus.Vacant;

        public DateTimeOffset? CreatedTime { get; set; }

        public Guid? CreatedBy { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ít nhất một hình ảnh")]
        public List<IFormFile> Images { get; set; } = new();
    }

}
