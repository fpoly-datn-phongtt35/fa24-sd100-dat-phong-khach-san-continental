//$(document).ready(function () {
//    setInterval(() => {
//        $.ajax({
//            type: 'POST',
//            url: '/RoomBooking/CheckDepositRoomBooking',
//            success: function (data) {
//            },
//            error: function (xhr, status, error) {
//                console.log("Error: " + error);
//            }
//        });
//    }, 3000);
//})
var global =
{
    Noti: async function (title ="Xác nhận cập nhật",txt = "Bạn có chắc chắn muốn cập nhật không?") {
        try {
            const result = await Swal.fire({
                title: title,
                text: txt,
                icon: 'question',
                html: txt.replace(/\n/g, '<br>'), // Thay thế \n bằng <br>
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Xác nhận',
                cancelButtonText: 'Hủy'
            });

            if (result.isConfirmed) {
                return 1;
            } else {
                return -1;
            }
        } catch (error) {
            console.error("Lỗi trong Noti:", error);
            return -1;
        }
    },

    ShowTabMenu: function (element) {
        var menu = element.parentElement.querySelector('ul');
        var i = element.parentElement.querySelector('i');
        i.classList.toggle('icon_rotate');
        menu.classList.toggle('show');
    },
    Account_DashBoard: function ()
    {
        $(".dashboard").toggle('show');
        if ($("#icon_up_dashboard").hasClass('show_dashboard')) {
            $("#icon_up_dashboard").removeClass('show_dashboard');

        }
        else {
            $("#icon_up_dashboard").addClass('show_dashboard');
        }
    },
    NumberVNFormated: function (number)
    {
        var formattedprice = parseFloat(number).toLocaleString('vi-VN');
        formattedprice = formattedprice.replaceAll('.', ',')
        return formattedprice;
    },
    createNewDateInVietnamTimezone: function () {
        // Tạo đối tượng Date mới với thời gian hiện tại
        const now = new Date();

        // Tính toán thời gian tại Việt Nam (GMT+7)
        const vietnamOffset = 7 * 60 * 60 * 1000; // 7 giờ * 60 phút * 60 giây * 1000 mili giây
        const vietnamTime = now.getTime() + vietnamOffset;

        // Tạo đối tượng Date mới với thời gian tại Việt Nam
        return new Date(vietnamTime);
    },
    formatDateToMMDDYYYY: function (date) {
        // Tạo một đối tượng Date mới để tránh thay đổi đối tượng gốc
        const newDate = new Date(date);

        // Lấy năm, tháng và ngày
        const year = newDate.getFullYear();
        const month = newDate.getMonth() + 1; // Tháng bắt đầu từ 0
        const day = newDate.getDate();

        // Tạo đối tượng Date mới với định dạng MM/DD/YYYY
        return new Date(year, month - 1, day);
    },
    getStatusText: function (statusId, statusMap) {
        return statusMap.get(statusId) || "Trạng thái không hợp lệ";
    },

    getResponseStatus: function (statusId, statusMap) {
        for (const status in statusMap) {
            if (statusMap[status] === statusId) {
                return status;
            }
        }
        return "Trạng thái không hợp lệ";
    },
    POST: function (url, data) {
        return new Promise(function (resolve, reject) {
            $.ajax({
                type: 'post',
                url: url,
                data: { request: data },
                success: function (data) {
                    resolve(data);
                },
                error: function (err) {
                    reject(err);
                }
            });
        });
    },


    GET: function (url) {
        return new Promise(function (resolve, reject) {
            $.ajax({
                url: url,
                dataType: 'json',
                type: 'get',
                contentType: 'application/json',
                processData: false,
                success: function (data) {
                    resolve(data);
                },
                error: function (err) {
                    reject(err);
                }
            });
        });
    },
    convertVietnameseToUnsign: function (str) {
        // Bảng chuyển đổi các ký tự có dấu thành không dấu
        const from = "àáạảãâầấậẩẫăắặẳẵèéẹẻẽêềếệểễìíịỉĩòóọỏõôồốộổỗơớờợởỡùúụủũưừứựửữỳýỵỷỹđ";
        const to = "aaaaaaaaaaaaaaaaeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyd";
        const fromArray = from.split('');
        const toArray = to.split('');

        str = str.toLowerCase();

        // Chuyển đổi các ký tự có dấu
        for (let i = 0; i < fromArray.length; i++) {
            str = str.replace(new RegExp(fromArray[i], 'g'), toArray[i]);
        }

        // Loại bỏ các ký tự đặc biệt ngoại trừ -
        str = str.replace(/[^\w\s-]/g, '');

        // Thay thế nhiều khoảng trắng thành 1 -
        str = str.replace(/\s+/g, '-');

        // Loại bỏ 2 dấu - liền nhau
        str = str.replace(/--+/g, '-');

        return str.trim();
    }
}