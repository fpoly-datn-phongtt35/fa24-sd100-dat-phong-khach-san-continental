window.addEventListener("scroll", function () {
    var searchBox = document.querySelector(".search-box");
    var bannerHeight = document.querySelector(".banner").offsetHeight;

    if (window.scrollY > bannerHeight - 80) {
        searchBox.classList.add("sticky");
    } else {
        searchBox.classList.remove("sticky");
    }
});
// Hàm định dạng ngày
function formatDate(date) {
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0'); // Tháng dạng số
    const year = date.getFullYear();
    return `${year}-${month}-${day}`;
}

// Lấy giá trị từ localStorage
const checkIn = localStorage.getItem("CheckIn");
const checkOut = localStorage.getItem("CheckOut");
const maxiumOccupancy = localStorage.getItem("maxiumOccupancy");
const quantityRoom = localStorage.getItem("quantityRoom");
document.addEventListener("DOMContentLoaded", function () {
    const checkInValue = document.getElementById('CheckInValue');
    const checkOutValue = document.getElementById('CheckOutValue');
    const maxOccupancyValue = document.getElementById('MaxOccupancyValue');
    const quantityRoomValue = document.getElementById('QuantityRoomValue');

    if (checkInValue) {
        checkInValue.value = checkIn;
    }
    if (checkOutValue) {
        checkOutValue.value = checkOut;
    }
    if (maxOccupancyValue) {
        maxOccupancyValue.value = maxiumOccupancy;
    }
    if (quantityRoomValue) {
        quantityRoomValue.value = quantityRoom;
    }
});
// Gán giá trị mặc định
document.getElementById('maxiumOccupancy').value = maxiumOccupancy || 1;
document.getElementById('quantityRoom').value = quantityRoom || 1;

const today = new Date();
const formattedToday = formatDate(today);

// Khởi tạo Flatpickr
document.addEventListener('DOMContentLoaded', function () {
    const tomorrow = new Date();
    tomorrow.setDate(tomorrow.getDate() + 1); // Ngày mai

    const minCheckOutDate = new Date(tomorrow);
    minCheckOutDate.setDate(minCheckOutDate.getDate() + 1); // Ngày trả phòng tối thiểu

    // Flatpickr cho checkIn
    const checkInPicker = $('#checkIn').flatpickr({
        dateFormat: "d/m/Y", // Định dạng số: ngày/tháng/năm (24/02/2025)
        minDate: "today",
        defaultDate: checkIn || formatDate(tomorrow),
        onChange: function (selectedDates) {
            validateDates();
            const minCheckOut = new Date(selectedDates[0]);
            minCheckOut.setDate(minCheckOut.getDate() + 1);
            checkOutPicker.set('minDate', minCheckOut);
        }
    });

    // Flatpickr cho checkOut
    const checkOutPicker = $('#checkOut').flatpickr({
        dateFormat: "d/m/Y",
        minDate: new Date().fp_incr(1),
        defaultDate: checkOut || formatDate(minCheckOutDate),
        onChange: function () {
            validateDates();
        }
    });

    document.getElementById('checkIn').setAttribute('min', formattedToday);
});

function saveToLocalStorage() {
    localStorage.setItem("CheckIn", document.getElementById('checkIn').value);
    localStorage.setItem("CheckOut", document.getElementById('checkOut').value);
    localStorage.setItem("maxiumOccupancy", document.getElementById('maxiumOccupancy').value);
    localStorage.setItem("quantityRoom", document.getElementById('quantityRoom').value);
}

// Kiểm tra ngày
function validateDates() {
    const checkInValue = document.getElementById('checkIn').value;
    const checkOutValue = document.getElementById('checkOut').value;
    const maxOccupancyValue = parseInt(document.getElementById('maxiumOccupancy').value, 10);
    const roomQuantityValue = parseInt(document.getElementById('quantityRoom').value, 10);

    if (checkInValue && checkOutValue) {
        // Giả sử định dạng là "dd/mm/yyyy" (24/02/2025)
        const [dayIn, monthIn, yearIn] = checkInValue.split('/');
        const checkInDate = new Date(`${yearIn}-${monthIn}-${dayIn}T14:00:00`);

        if (isNaN(checkInDate.getTime())) {
            console.error("Invalid checkInDate:", checkInDate);
            return false;
        }

        const [dayOut, monthOut, yearOut] = checkOutValue.split('/');
        const checkOutDate = new Date(`${yearOut}-${monthOut}-${dayOut}T12:00:00`);

        if (isNaN(checkOutDate.getTime())) {
            console.error("Invalid checkOutDate:", checkOutDate);
            return false;
        }

        if (checkOutDate <= checkInDate) {
            showToast("Thời gian trả phòng phải lớn hơn thời gian nhận phòng 1 ngày!");
            return false;
        }

        if (roomQuantityValue > maxOccupancyValue) {
            showToast("Số lượng người phải lớn hơn số lượng phòng!");
            return false;
        }

        return true;
    }
    return false;
}

function showToast(message) {
    const toastHTML = `
        <div class="toast" role="alert" aria-live="assertive" aria-atomic="true" style="position: absolute; top: 120px; right: 10px; z-index: 1050;">
            <div class="toast-header">
                <strong class="me-auto">Thông báo</strong>
                <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
            </div>
            <div class="toast-body">${message}</div>
        </div>
    `;
    $('body').append(toastHTML);
    $('.toast').toast({ delay: 3000 }).toast('show');
    setTimeout(() => { $('.toast').remove(); }, 6200);
}

document.getElementById('maxiumOccupancy').addEventListener('change', validateDates);
document.getElementById('quantityRoom').addEventListener('change', validateDates);
document.getElementById('validateButton').addEventListener('click', function (event) {
    event.preventDefault();
    if (validateDates()) {
        saveToLocalStorage();
        document.querySelector('form').submit();
    }
});