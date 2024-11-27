$(document).ready(function () {
    function showToast(toastId) {
        var toastEl = document.getElementById(toastId);
        var toast = new bootstrap.Toast(toastEl);
       /* toast.show();*/
    }

    $('#openModalButton').click(function () {
        $('#bookingModal').modal('show');
    });
    const checkIn = localStorage.getItem("CheckIn");
    const checkOut = localStorage.getItem("CheckOut");
    $('#checkInBooking, #checkOutBooking').change(function () {
      
        var pricePerNight = roomPrice;

        // Calculate number of days
        if (checkIn && checkOut && checkOut > checkIn) {
            var timeDifference = checkOut - checkIn;
            var days = Math.ceil(timeDifference / (1000 * 3600 * 24));
            var totalPayment = days * pricePerNight;
            var depositPayment = totalPayment * 20 / 100;
            $('#depositPayment').text("Đặt cọc: " + depositPayment+ " VNĐ")
            $('#totalPayment').text("Thanh toán: " + totalPayment + " VNĐ");
        } else {
            $('#depositPayment').text("Đặt cọc: 0 VNĐ")
            $('#totalPayment').text("Thanh toán: 0 VNĐ");
        }
    });

    $('#confirmBookingButton').click(function () {
        $('#validationMessage').html('').hide();

        if (!isUserLoggedIn) {
            $('#validationMessage').html("Vui lòng đăng nhập để đặt phòng.");
            console.log("User is not logged in, showing error toast");
            showToast('errorToastLogin');
            $('#validationMessage').show();
            return;
        }

        var checkIn = new Date($('#checkInBooking').val());
        var checkOut = new Date($('#checkOutBooking').val());
        var now = new Date();

        // Kiểm tra điều kiện
        var fiveHoursFromNow = new Date(now.getTime() + 5 * 60 * 60 * 1000);
        var validationMessage = "";

        if (checkIn < Date.now) {
            validationMessage += "Thời gian checkin phải không được nhỏ hơn hiện tại.<br>";
        }
        if (checkIn < fiveHoursFromNow) {
            validationMessage += "Thời gian checkin phải cách thời điểm hiện tại 5 tiếng.<br>";
        }
        if (checkOut <= checkIn) {
            validationMessage += "Thời gian checkout không được nhỏ hơn thời gian checkin.<br>";
        }

        if (validationMessage) {
            $('#validationMessage').html(validationMessage);
        } else {
            // Nếu hợp lệ, thực hiện gửi dữ liệu booking
            var bookingDetails = {
                RoomId: roomId,
                CheckInBooking: $('#checkInBooking').val(),
                CheckOutBooking: $('#checkOutBooking').val(),
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