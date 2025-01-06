$(document).ready(function () {
    if (!isUserLoggedIn) {
        function checkCustomerInfo() {
            var firstName = $('#firstName').val().trim();
            var lastName = $('#lastName').val().trim();
            var email = $('#email').val().trim();
            var phoneNumber = $('#phone').val().trim();

            // Kiểm tra nếu tất cả thông tin đều đã được nhập
            if (firstName && lastName && email && phoneNumber) {
                $('#confirmBookingButton').prop('disabled', false); // Kích hoạt nút
            } else {
                $('#confirmBookingButton').prop('disabled', true); // Vô hiệu hóa nút
            }
        }

        $('#firstName, #lastName, #email, #phone').on('input change', function () {
            checkCustomerInfo();
        });

        // Kiểm tra khi trang được tải
        checkCustomerInfo();
    }
   

    function formatDate(date) {
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');
        return `${day}-${month}-${year}`;
    }

    // Lấy giá trị từ localStorage ngay khi trang được tải
    const checkIn = localStorage.getItem("CheckIn");
    const checkOut = localStorage.getItem("CheckOut");
    var pricePerNight = roomPrice;
    // Cập nhật nội dung cho checkIn và checkOut
    $('#checkIn').text("Từ 14:00 " + checkIn);
    $('#checkOut').text("Trước 12:00 " + checkOut);
    function parseDate(dateString) {
        const parts = dateString.split('-');
        return new Date(parts[2], parts[1] - 1, parts[0]);
    }
    // Kiểm tra điều kiện checkIn và checkOut
    if (checkIn && checkOut && checkOut > checkIn) {
        const checkInDate = parseDate(checkIn);
        const checkOutDate = parseDate(checkOut);
        var days = Math.ceil((checkOutDate - checkInDate) / (1000 * 3600 * 24));
        var totalPayment = Math.round(days * pricePerNight);
        var depositPayment = Math.round(totalPayment * 0.2);

        // Cập nhật thông tin giao diện
        $('#depositPayment').text("Đặt cọc: " + depositPayment.toLocaleString() + " VNĐ");
        $('#totalRoomPayment').text("Tiền phòng: " + totalPayment.toLocaleString() + " VNĐ");
        updateTotalServicePayment(); // Cập nhật tổng tiền dịch vụ
    } else {
        $('#depositPayment').text("Đặt cọc: 0 VNĐ");
        $('#totalRoomPayment').text("Tiền phòng: 0 VNĐ");
    }


    // Hàm để cập nhật tổng tiền dịch vụ
    function updateTotalServicePayment() {
        var totalServicePayment = 0;

        // Kiểm tra nếu có dịch vụ nào được chọn
        var selectedServices = $('.service-checkbox:checked');
        if (selectedServices.length > 0) {
            selectedServices.each(function () {
                var servicePrice = parseFloat($(this).closest('.form-check').find('span').text().replace(' VNĐ', '').replace(/,/g, ""));
                var quantity = parseInt($('#quantity_' + $(this).val()).val(), 10);
                totalServicePayment += servicePrice * quantity;
            });
        }

        $('#totalServicePayment').text("Tiền dịch vụ: " + totalServicePayment.toLocaleString() + " VNĐ");
        updateTotalPrice(); // Cập nhật tổng giá
    }

    // Hàm cập nhật tổng giá
    function updateTotalPrice() {
        var depositPaymentText = $('#depositPayment').text();
        var depositPayment = parseFloat(depositPaymentText.replace("Đặt cọc: ", "").replace(" VNĐ", "").replace(/,/g, ""));
        var roomPriceText = $('#totalRoomPayment').text();
        var roomPrice = parseFloat(roomPriceText.replace("Tiền phòng: ", "").replace(" VNĐ", "").replace(/,/g, ""));
        var servicePriceText = $('#totalServicePayment').text();
        var servicePrice = parseFloat(servicePriceText.replace("Tiền dịch vụ: ", "").replace(" VNĐ", "").replace(/,/g, ""));

        var totalPrice = Math.round(roomPrice + servicePrice - depositPayment);
        $('#totalPrice').text("Tổng tiền sau khi đặt cọc: " + totalPrice.toLocaleString() + " VNĐ");
    }

    // Cập nhật tổng tiền dịch vụ khi có sự thay đổi
    $('.service-checkbox').change(function () {
        updateTotalServicePayment();
    });

    $('.service-quantity').on('input change', function () {
        updateTotalServicePayment();
    });

    // Xác nhận đặt phòng
    document.getElementById('confirmBookingButton').addEventListener('click', function () {
        //var email = document.getElementById('email').value;
        //var phoneNumber = document.getElementById('phone').value;
        var roomName = document.querySelector('.card-title').innerText;
        var totalPrice = document.getElementById('totalPrice').innerText.replace('Tổng tiền sau khi đặt cọc: ', '').replace(' VNĐ', '').trim();

        // Lấy danh sách dịch vụ đã chọn
        var services = [];
        document.querySelectorAll('.service-checkbox:checked').forEach(function (checkbox) {
            var serviceName = checkbox.nextElementSibling.querySelector('strong').innerText;
            var quantity = document.getElementById('quantity_' + checkbox.value).value;
            services.push(serviceName + ' (x' + quantity + ')');
        });

        // Cập nhật thông tin vào modal
        document.getElementById('modalRoomName').innerText = roomName;
        document.getElementById('modalCheckIn').value = checkIn;
        document.getElementById('modalCheckOut').value = checkOut;
        //document.getElementById('modalEmail').innerText = email;
        //document.getElementById('modalPhoneNumber').innerText = phoneNumber;
        document.getElementById('modalDeposit').innerText = depositPayment + ' VNĐ';
        document.getElementById('modalTotalPrice').innerText = totalPrice + ' VNĐ';
        document.getElementById('modalServices').innerText = services.join(', ') || 'Không có dịch vụ nào';

        // Hiển thị modal
        $('#bookingConfirmationModal').modal('show');
    });
    $('#confirmBooking').click(function () {
        $('#validationMessage').html('').hide();

        var depositText = $('#depositPayment').text();
        var deposit = parseFloat(depositText.replace("Đặt cọc: ", "").replace(" VNĐ", "").replace(/,/g, ""));
        var priceText = $('#totalRoomPayment').text();
        var price = parseFloat(priceText.replace("Tiền phòng: ", "").replace(" VNĐ", "").replace(/,/g, ""));
        var servicePriceText = $('#totalServicePayment').text();
        var servicePrice = parseFloat(servicePriceText.replace("Tiền dịch vụ: ", "").replace(" VNĐ", "").replace(/,/g, ""));

        // Tạo đối tượng bookingDetails
        var bookingDetails = {
            RoomId: roomId,
            CheckInBooking: new Date(localStorage.getItem("CheckIn")),
            CheckOutBooking: new Date(localStorage.getItem("CheckOut")),
            Price: Math.round(price),
            SelectedServices: [],
            Customer: {
                FirstName: $('#firstName').val() || null,
                LastName: $('#lastName').val() || null,
                Email: $('#email').val() || null,
                PhoneNumber: $('#phone').val() || null
            },
            Deposit: Math.round(deposit),
            ServicePrice: Math.round(servicePrice)
        };

        // Lấy danh sách dịch vụ đã chọn
        $('.service-checkbox:checked').each(function () {
            var serviceId = $(this).val();
            var quantity = parseInt($('#quantity_' + serviceId).val(), 10);
            var servicePrice = parseFloat($(this).closest('.form-check').find('span').data('price'));
            bookingDetails.SelectedServices.push({ ServiceId: serviceId, Quantity: quantity, Price: Math.round(servicePrice) });
        });

        // Gửi dữ liệu đặt phòng
        $.ajax({
            url: bookingUrl,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(bookingDetails),
            success: function (response) {
                if (response.success) {
                    var roomBookingId = response.roomBookingId; // Lấy RoomBookingId từ response
                    showToast("Đặt phòng thành công!");

                    // Tạo form ẩn và tự động submit để chuyển hướng sang CreatePaymentLink
                    var form = $('<form action="/RoomBooking/CreatePaymentLink" method="POST"></form>');
                    form.append('<input type="hidden" name="RoomBookingId" value="' + roomBookingId + '">');
                    $('body').append(form);
                    form.submit();
                } else {
                    $('#validationMessage').html("Đã xảy ra lỗi: " + (response.message || "Không thể đặt phòng.")).show();
                }
            },
            error: function (xhr, status, error) {
                showToast("Hãy kiểm tra lại thông tin cá nhân, thông tin đặt phòng!");
                console.log("Error: " + error);
                $('#validationMessage').html("Đã xảy ra lỗi trong quá trình đặt phòng: " + xhr.responseText).show();
            }
        });
    });

    function showToast(message) {
        var toastHTML = `
            <div class="toast" role="alert" aria-live="assertive" aria-atomic="true" style="position: absolute; top: 100px; right: 10px; z-index: 2050;">
                <div class="toast-header">
                    <strong class="mr-auto">Thông báo</strong>
                </div>
                <div class="toast-body">
                    ${message}
                </div>
            </div>
        `;
        $('body').append(toastHTML); // Thêm toast vào body
        $('.toast').toast({ delay: 3000 }).toast('show'); // Hiển thị toast

        // Tự động xóa toast sau khi hiển thị
        setTimeout(function () {
            $('.toast').remove();
        }, 3200);
    }
});