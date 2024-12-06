function formatDate(date) {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
}

const today = new Date();

// Lấy giá trị từ localStorage cho checkIn, checkOut, maxiumOccupancy và quantityRoom
const checkIn = localStorage.getItem("CheckIn");
const checkOut = localStorage.getItem("CheckOut");
const maxiumOccupancy = localStorage.getItem("maxiumOccupancy");
const quantityRoom = localStorage.getItem("quantityRoom");

// Chuyển đổi giá trị từ localStorage thành Date nếu có giá trị
if (checkIn) {
    const checkInDateObject = new Date(checkIn);
    document.getElementById('checkIn').value = formatDate(checkInDateObject); // Định dạng lại
} else {
    const checkInDate = new Date(today);
    checkInDate.setDate(today.getDate() + 1);
    document.getElementById('checkIn').value = formatDate(checkInDate);
}

// Kiểm tra nếu có giá trị checkOut trong localStorage
if (checkOut) {
    const checkOutDateObject = new Date(checkOut);
    document.getElementById('checkOut').value = formatDate(checkOutDateObject); // Định dạng lại
} else {
    const checkOutDate = new Date(today);
    checkOutDate.setDate(today.getDate() + 2);
    document.getElementById('checkOut').value = formatDate(checkOutDate);
}

// Lưu giá trị cho maxiumOccupancy và quantityRoom vào input
if (maxiumOccupancy) {
    document.getElementById('maxiumOccupancy').value = maxiumOccupancy;
} else {
    document.getElementById('maxiumOccupancy').value = 1;
}

if (quantityRoom) {
    document.getElementById('quantityRoom').value = quantityRoom;
} else {
    document.getElementById('quantityRoom').value = 1;
}

// Đặt giá trị min cho checkIn
const formattedToday = formatDate(today);
document.getElementById('checkIn').setAttribute('min', formattedToday);

// Hàm lưu giá trị vào localStorage
function saveToLocalStorage() {
    localStorage.setItem("CheckIn", document.getElementById('checkIn').value);
    localStorage.setItem("CheckOut", document.getElementById('checkOut').value);
    localStorage.setItem("maxiumOccupancy", document.getElementById('maxiumOccupancy').value);
    localStorage.setItem("quantityRoom", document.getElementById('quantityRoom').value);
}

function validateDates() {
    const checkInValue = document.getElementById('checkIn').value;
    const checkOutValue = document.getElementById('checkOut').value;
    const maxOccupancyValue = parseInt(document.getElementById('maxiumOccupancy').value, 10);
    const roomQuantityValue = parseInt(document.getElementById('quantityRoom').value, 10);

    const checkInDate = new Date(checkInValue);
    const checkOutDate = new Date(checkOutValue);
    const minCheckIn = formatDate(today);
    const minCheckOutDate = new Date(checkInDate);
    minCheckOutDate.setDate(checkInDate.getDate() + 1); // Ngày trả phòng phải lớn hơn ngày nhận phòng ít nhất 1 ngày
    if (checkOutValue <= minCheckIn) {
        document.getElementById('checkOut').value = formatDate(minCheckOutDate);
        showToast("Thời gian trả phòng phải lớn hơn thời gian nhận phòng 1 ngày!");
        return false;
    }
    if (checkOutValue <= checkInValue) {
        showToast("Thời gian trả phòng phải lớn hơn thời gian nhận phòng 1 ngày!");
        return false;
    }
    if (roomQuantityValue > maxOccupancyValue) {
        document.getElementById('quantityRoom').value = maxOccupancyValue;
        showToast("Số lượng người phải lớn hơn số lượng phòng!");
        return false;
    }

    document.getElementById('checkOut').setAttribute('min', checkInValue);
    return true;
}
document.addEventListener('DOMContentLoaded', function () {
    const today = new Date();
    const tomorrow = new Date(today);
    tomorrow.setDate(today.getDate() + 1);
    flatpickr("#checkIn", {
        plugins: [new rangePlugin({ input: "#checkOut" })],
        dateFormat: "Y-m-d",
        minDate: today
    });
    flatpickr("#checkOut", {
        dateFormat: "Y-m-d",
        minDate: tomorrow
    });
});
function showToast(message) {
    var toastHTML = `
            <div class="toast" role="alert" aria-live="assertive" aria-atomic="true" style="position: absolute; top: 120px; right: 10px; z-index: 1050;">
                <div class="toast-header">
                    <strong class="me-auto">Thông báo</strong>
                    <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
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
    }, 6200);
}

document.getElementById('checkIn').addEventListener('change', validateDates);
document.getElementById('checkOut').addEventListener('change', validateDates);
document.getElementById('maxiumOccupancy').addEventListener('change', validateDates);
document.getElementById('quantityRoom').addEventListener('change', validateDates)
document.getElementById('validateButton').addEventListener('click', function (event) {
    if (validateDates()) {
        saveToLocalStorage(); // Lưu vào localStorage khi xác thực thành công
    } else {
        event.preventDefault(); // Ngăn gửi form nếu không hợp lệ
    }
});
