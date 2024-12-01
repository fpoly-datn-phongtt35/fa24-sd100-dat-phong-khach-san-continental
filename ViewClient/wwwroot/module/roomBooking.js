$(document).ready(function () {
    $('#openModalButton').click(function () {
        $('#bookingModal').modal('show');

        var checkIn = new Date(localStorage.getItem("CheckIn"));
        var checkOut = new Date(localStorage.getItem("CheckOut"));

        $('#checkIn').text("Từ 14:00 " + formatDate(checkIn));
        $('#checkOut').text("Trước 12:00 " + formatDate(checkOut));

        updateBookingDates(checkIn, checkOut);
    });

    function formatDate(date) {
        var day = String(date.getDate()).padStart(2, '0');
        var month = String(date.getMonth() + 1).padStart(2, '0');
        var year = date.getFullYear();
        return day + '-' + month + '-' + year;
    }

    function updateBookingDates(checkIn, checkOut) {
        var pricePerNight = roomPrice;

        if (checkIn && checkOut && checkOut > checkIn) {
            var days = Math.ceil((checkOut - checkIn) / (1000 * 3600 * 24));
            var totalPayment = days * pricePerNight;
            var depositPayment = totalPayment * 0.2;

            $('#depositPayment').text("Đặt cọc: " + depositPayment.toLocaleString() + " VNĐ");
            $('#totalRoomPayment').text("Tiền phòng: " + totalPayment.toLocaleString() + " VNĐ");
        } else {
            $('#depositPayment').text("Đặt cọc: 0 VNĐ");
            $('#totalRoomPayment').text("Tiền phòng: 0 VNĐ");
        }
    }

    $('#checkIn, #checkOut').change(function () {
        var checkIn = new Date($('#checkIn').text().replace("Từ 14:00 ", ""));
        var checkOut = new Date($('#checkOut').text().replace("Trước 12:00 ", ""));
        localStorage.setItem("CheckIn", checkIn);
        localStorage.setItem("CheckOut", checkOut);
        updateBookingDates(checkIn, checkOut);
    });

    $('#confirmBookingButton').click(function () {
        $('#validationMessage').html('').hide();

        // Lấy giá phòng
        var priceText = $('#totalRoomPayment').text();
        var price = parseFloat(priceText.replace("Tiền phòng: ", "").replace(" VNĐ", "").replace(/,/g, ""));

        // Tạo đối tượng bookingDetails
        var bookingDetails = {
            RoomId: roomId,
            CheckInBooking: new Date(localStorage.getItem("CheckIn")).toISOString(),
            CheckOutBooking: new Date(localStorage.getItem("CheckOut")).toISOString(),
            Price: price,
            SelectedServices: []
        };

        // Lấy danh sách dịch vụ đã chọn
        $('.service-checkbox:checked').each(function () {
            var serviceId = $(this).val();
            var quantity = parseInt($('#quantity_' + serviceId).val(), 10);
            var servicePrice = parseFloat($(this).closest('.form-check').find('span').text().replace(' VNĐ', '').replace(/\./g, ""));
            bookingDetails.SelectedServices.push({ ServiceId: serviceId, Quantity: quantity, Price: servicePrice });
        });

        // Gửi dữ liệu đặt phòng
        $.ajax({
            url: bookingUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(bookingDetails),
            success: function (response) {
                $('#modalContainer').html(response);
                $('#bookingModal').modal('hide');
                setTimeout(function () {
                    window.location.href = '/Home/Index';
                }, 2000);
            },
            error: function (xhr, status, error) {
                console.log("Error: " + error); // In ra thông tin lỗi
                $('#validationMessage').html("Đã xảy ra lỗi trong quá trình đặt phòng: " + xhr.responseText).show();
            }
        });
    });
});