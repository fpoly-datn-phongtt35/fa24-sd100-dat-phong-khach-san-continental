'use strict';

$(document).ready(function () {
    function executeAction(actionUrl, roomBookingDetailId, additionalData = {}, successCallback, errorCallback) {
        $.ajax({
            url: actionUrl,
            method: 'POST',
            data: { id: roomBookingDetailId, ...additionalData },
            success: successCallback,
            error: errorCallback || function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Có lỗi xảy ra!',
                    text: 'Không thể kết nối tới server!'
                });
            }
        });
    }
    function handleCheckIn(roomBookingDetailId) {
        function executeCheckIn(forceCheckIn = false) {
            executeAction(
                '/RoomBooking/CheckIn2',
                roomBookingDetailId,
                { forceCheckIn: forceCheckIn },
                function (response) {
                    if (response.success) {
                        Swal.fire({
                            icon: 'success',
                            title: response.message
                        }).then(function () {
                            location.reload();
                        });
                    } else if (response.message.includes("sớm hơn ngày đặt") && !forceCheckIn) {
                        Swal.fire({
                            title: "Bạn đang check-in sớm hơn ngày đặt.",
                            text: "Bạn có muốn tiếp tục check-in không?",
                            icon: "warning",
                            showCancelButton: true,
                            confirmButtonColor: "#3085d6",
                            cancelButtonColor: "#d33",
                            confirmButtonText: "Xác nhận",
                            cancelButtonText: "Hủy"
                        }).then(function (result) {
                            if (result.isConfirmed) {
                                executeCheckIn(true); // Thực hiện check-in với xác nhận
                            } else if (result.isDismissed) {
                                Swal.fire({
                                    icon: 'info',
                                    title: 'Hủy bỏ check-in',
                                    text: 'Bạn đã hủy check-in thành công.'
                                });
                            }
                        });
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Có lỗi xảy ra',
                            text: response.message
                        });
                    }
                }
            );
        }

        executeCheckIn();
    }
    function handleCheckOut(roomBookingDetailId) {
        executeAction(
            '/RoomBooking/CheckOut2',
            roomBookingDetailId,
            {},
            function (response) {
                if (response.success) {
                    Swal.fire({
                        icon: 'success',
                        title: response.message
                    }).then(function () {
                        location.reload();
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Có lỗi xảy ra',
                        text: response.message
                    });
                }
            }
        );
    }
    
    $('#checkInButton').on('click', function () {
        const roomBookingDetailId = $(this).data('id');
        handleCheckIn(roomBookingDetailId);
    });
    
    $('#checkOutButton').on('click', function () {
        const roomBookingDetailId = $(this).data('id');
        handleCheckOut(roomBookingDetailId);
    });
});

$(document).ready(function () {
    function executeAction(actionUrl, data, successCallback, errorCallback) {
        $.ajax({
            url: actionUrl,
            method: 'POST',
            data: data,
            success: successCallback,
            error: errorCallback || function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Có lỗi xảy ra!',
                    text: 'Không thể kết nối tới server!'
                });
            }
        });
    }

    function handleUpdateCheckInAndCheckOut(roomBookingDetailId, checkInDateTime, checkOutDateTime, note, expenses) {
        executeAction(
            '/RoomBooking/UpdateCheckInAndCheckOutReality',
            {
                id: roomBookingDetailId,
                checkInTime: checkInDateTime,
                checkoutTime: checkOutDateTime,
                note: note,
                expenses: expenses
            },
            function (response) {
                if (response.success) {
                    Swal.fire({
                        icon: 'success',
                        title: response.message
                    }).then(function () {
                        location.reload();
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Có lỗi xảy ra',
                        text: response.message
                    });
                }
            }
        );
    }

    $('#btnUpdate').click(function () {
        const roomBookingDetailId = $(this).data('room-booking-detail-id');
        const selectedCheckInDateTime = $('#checkInRealityPicker').val();
        const selectedCheckOutDateTime = $('#checkOutRealityPicker').val();
        const note = $('#Note').val().trim();

        // Lấy giá trị phí hư tổn và loại bỏ dấu phẩy (nếu có), sau đó chuyển thành số
        const expensesInput = $('#Expenses').val().replace(/,/g, '');  // Loại bỏ dấu phẩy
        const expenses = parseFloat(expensesInput) || 0;  // Chuyển thành số thực

        if (expenses > 0 && !note) {
            Swal.fire({
                icon: 'warning',
                title: 'Thông báo',
                text: 'Vui lòng nhập ghi chú khi thêm phí hư tổn.'
            });
            return;
        }

        if (!roomBookingDetailId) {
            Swal.fire({
                icon: 'error',
                title: 'Lỗi',
                text: 'Không có Id phòng hợp lệ.'
            });
            return;
        }

        Swal.fire({
            title: 'Xác nhận cập nhật',
            text: 'Bạn có chắc chắn muốn cập nhật không?',
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Xác nhận',
            cancelButtonText: 'Hủy'
        }).then(function (result) {
            if (result.isConfirmed) {
                handleUpdateCheckInAndCheckOut(roomBookingDetailId, selectedCheckInDateTime, selectedCheckOutDateTime, 
                    note, expenses);
            }
        });
    });

    
    $('#checkInRealityPicker').flatpickr({
        enableTime: true,           
        noCalendar: false,            
        dateFormat: "d/m/Y h:i K",  
        time_24hr: false,            
        defaultHour: 8,            
        defaultMinute: 0, 
        minuteIncrement: 1,
        disableMobile: true, 
        static: true,
    });

    $('#checkOutRealityPicker').flatpickr({
        enableTime: true,
        noCalendar: false,
        dateFormat: "d/m/Y h:i K",
        time_24hr: false,
        defaultHour: 8,
        defaultMinute: 0,
        minuteIncrement: 1,
        disableMobile: true,
        static: true,
    });
});

function validateExpenses() {
    const expensesInput = document.getElementById('Expenses');
    let value = expensesInput.value;

    value = value.replace(/[^0-9,]/g, '');

    if (value.includes(',')) {
        value = value.replace(/,/g, '');
    }

    if (parseFloat(value) < 0) {
        value = '0';
    }

    expensesInput.value = value;
}




