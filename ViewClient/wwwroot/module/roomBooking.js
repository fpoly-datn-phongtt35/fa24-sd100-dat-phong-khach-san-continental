$(document).ready(function () {
    function showToast(toastId) {
        var toastEl = document.getElementById(toastId);
        var toast = new bootstrap.Toast(toastEl);
        // toast.show();  // Uncomment this line to show the toast
    }
   

    $('#openModalButton').click(function () {
        $('#bookingModal').modal('show');

        // Lấy giá trị checkIn và checkOut từ localStorage khi mở modal
        var checkIn = new Date(localStorage.getItem("CheckIn"));
        var checkOut = new Date(localStorage.getItem("CheckOut"));

        // Cập nhật giá trị vào các trường input
        $('#checkIn').val(checkIn);
        $('#checkOut').val(checkOut);

        // Gọi hàm để tính toán giá tiền khi modal mở
        updateBookingDates(checkIn, checkOut);
        $.get('/Room/IndexService', {})
            .done(function (data) {
                if (data.html) {
                    $('#serviceContainer').html(data.html);
                } else if (data.error) {
                    $('#serviceContainer').html("<p class='text-danger'>" + data.error + "</p>");
                }
            })
            .fail(function () {
                $('#serviceContainer').html("<p class='text-danger'>Đã xảy ra lỗi khi tải dịch vụ.</p>");
            });
    });
    function formatDate(date) {
        var day = String(date.getDate()).padStart(2, '0'); // Lấy ngày và thêm số 0 nếu cần
        var month = String(date.getMonth() + 1).padStart(2, '0'); // Lấy tháng (tháng bắt đầu từ 0)
        var year = date.getFullYear(); // Lấy năm
        return day + '-' + month + '-' + year; // Trả về định dạng dd-mm-yyyy
    }
    function updateBookingDates(checkIn, checkOut) {
        var pricePerNight = roomPrice;

        // Chuyển đổi checkIn và checkOut thành mốc thời gian
        var checkInDate = new Date(checkIn).getTime();
        var checkOutDate = new Date(checkOut).getTime();

        // Tính toán số ngày
        if (checkInDate && checkOutDate && checkOutDate > checkInDate) {
            var timeDifference = checkOutDate - checkInDate;
            var days = Math.ceil(timeDifference / (1000 * 3600 * 24));
            var totalPayment = days * pricePerNight;
            var depositPayment = totalPayment * 20 / 100;
            $('#depositPayment').text("Đặt cọc: " + depositPayment + " VNĐ");
            $('#totalRoomPayment').text("Thanh toán: " + totalPayment + " VNĐ");
            $('#checkIn').text("Từ 14:00 " + formatDate(checkIn));
            $('#checkOut').text("Trước 12:00 " + formatDate(checkOut));
        } else {
            $('#depositPayment').text("Đặt cọc: 0 VNĐ");
            $('#totalRoomPayment').text("Thanh toán: 0 VNĐ");
        }
    }

    $('#checkIn, #checkOut').change(function () {
        // Cập nhật lại giá trị trong localStorage khi người dùng thay đổi
        var checkIn = $('#checkIn').val();
        var checkOut = $('#checkOut').val();
        localStorage.setItem("CheckIn", checkIn);
        localStorage.setItem("CheckOut", checkOut);

        // Gọi hàm để tính toán giá tiền
        updateBookingDates(checkIn, checkOut);
    });

    $('#confirmBookingButton').click(function () {
        $('#validationMessage').html('').hide();

        // Kiểm tra thông tin và hiển thị thông báo nếu có
        if (validationMessage) {
            $('#validationMessage').html(validationMessage).show();
        } else {
            // Nếu hợp lệ, thực hiện gửi dữ liệu booking
            var bookingDetails = {
                RoomId: roomId,
                CheckInBooking: new Date(localStorage.getItem("CheckIn")).toISOString(), // Chuyển đổi về định dạng ISO
                CheckOutBooking: new Date(localStorage.getItem("CheckOut")).toISOString(), // Chuyển đổi về định dạng ISO
                Price: parseFloat($('#totalPayment').text().replace("Thanh toán: ", "").replace(" VNĐ", ""))
            };

            $.post(bookingUrl, bookingDetails, function (response) {
                // Xử lý phản hồi từ server
                $('#modalContainer').html(response);
                $('#bookingModal').modal('hide');

                setTimeout(function () {
                    window.location.href = '/Home/Index';
                }, 2000);
            }).fail(function () {
                $('#validationMessage').html("Đã xảy ra lỗi trong quá trình đặt phòng.").show();
                showToast('errorToastLogin');
            });
        }
    });
});