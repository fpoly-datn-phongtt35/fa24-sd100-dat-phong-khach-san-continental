function formatDate(date) {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${day}-${month}-${year}`;
}

const today = new Date();

// Lấy giá trị từ localStorage cho checkIn, checkOut, maxiumOccupancy và quantityRoom
const checkIn = localStorage.getItem("CheckIn");
const checkOut = localStorage.getItem("CheckOut");
const maxiumOccupancy = localStorage.getItem("maxiumOccupancy");
const quantityRoom = localStorage.getItem("quantityRoom");

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

    // Chuyển đổi checkIn và checkOut từ định dạng d-m-Y sang YYYY-MM-DD
    const [dayIn, monthIn, yearIn] = checkInValue.split('-');
    const checkInDate = new Date(`${yearIn}-${monthIn}-${dayIn}T00:00:00`);

    const [dayOut, monthOut, yearOut] = checkOutValue.split('-');
    const checkOutDate = new Date(`${yearOut}-${monthOut}-${dayOut}T00:00:00`);

    // Ngày hôm nay
    const today = new Date();
    today.setHours(0, 0, 0, 0); // Đặt giờ, phút, giây về 0

    // Ngày trả phòng tối thiểu
    const minCheckOutDate = new Date(checkInDate);
    minCheckOutDate.setDate(checkInDate.getDate() + 1); // Ngày trả phòng phải lớn hơn ngày nhận phòng ít nhất 1 ngày
    console.log(minCheckOutDate);

    if (checkOutDate <= minCheckOutDate) {
        document.getElementById('checkOut').value = formatDate(minCheckOutDate);
        showToast("Thời gian trả phòng phải lớn hơn thời gian nhận phòng 1 ngày!");
        return false;
    }
    if (checkOutDate <= checkInDate) {
        showToast("Thời gian trả phòng phải lớn hơn thời gian nhận phòng!");
        return false;
    }
    if (roomQuantityValue > maxOccupancyValue) {
        document.getElementById('quantityRoom').value = maxOccupancyValue;
        showToast("Số lượng người phải lớn hơn số lượng phòng!");
        return false;
    }

    document.getElementById('checkOut').setAttribute('min', formatDate(minCheckOutDate));
    return true;
}   
document.addEventListener('DOMContentLoaded', function () {
    const today = new Date();
    const tomorrow = new Date(today);
    tomorrow.setDate(today.getDate() + 1);

    if (checkIn) {
        document.getElementById('checkIn').value = checkIn;
    } else {
        // Nếu không có, thiết lập giá trị mặc định là ngày mai
        document.getElementById('checkIn').value = formatDate(tomorrow);
    }

    // Nếu có giá trị checkOut trong localStorage, thiết lập giá trị cho input
    if (checkOut) {
        document.getElementById('checkOut').value = checkOut;
    } else {
        // Nếu không có, thiết lập giá trị mặc định là ngày mốt (ngày nhận phòng + 1)
        const minCheckOutDate = new Date(tomorrow);
        minCheckOutDate.setDate(minCheckOutDate.getDate() + 1);
        document.getElementById('checkOut').value = formatDate(minCheckOutDate);
    }

    $('#checkIn').flatpickr({
        plugins: [new rangePlugin({ input: "#checkOut" })],
        noCalendar: false,
        dateFormat: "d-m-Y",
        time_24hr: true,
        defaultHour: 14,
        defaultMinute: 0,
        minuteIncrement: 1,
        disableMobile: true,
        static: true,
    });
    $('#checkOut').flatpickr({
        noCalendar: false,
        dateFormat: "d-m-Y",
        time_24hr: true,
        defaultHour: 12,
        defaultMinute: 0,
        minuteIncrement: 1,
        disableMobile: true,
        static: true,
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