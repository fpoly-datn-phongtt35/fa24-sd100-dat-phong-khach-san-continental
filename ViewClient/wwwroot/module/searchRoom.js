function formatDate(date) {
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();
    return `${year}-${month}-${day}`;
}

const today = new Date();

// Retrieve values from localStorage
const checkIn = localStorage.getItem("CheckIn");
const checkOut = localStorage.getItem("CheckOut");
const maxiumOccupancy = localStorage.getItem("maxiumOccupancy");
const quantityRoom = localStorage.getItem("quantityRoom");

// Set default values for maxiumOccupancy and quantityRoom
document.getElementById('maxiumOccupancy').value = maxiumOccupancy || 1;
document.getElementById('quantityRoom').value = quantityRoom || 1;

// Set minimum value for checkIn
const formattedToday = formatDate(today);
document.getElementById('checkIn').setAttribute('min', formattedToday);

// Save values to localStorage
function saveToLocalStorage() {
    localStorage.setItem("CheckIn", document.getElementById('checkIn').value);
    localStorage.setItem("CheckOut", document.getElementById('checkOut').value);
    localStorage.setItem("maxiumOccupancy", document.getElementById('maxiumOccupancy').value);
    localStorage.setItem("quantityRoom", document.getElementById('quantityRoom').value);
}

function convertMonth(monthString) {
    const months = {
        'Jan': '01',
        'Feb': '02',
        'Mar': '03',
        'Apr': '04',
        'May': '05',
        'Jun': '06',
        'Jul': '07',
        'Aug': '08',
        'Sep': '09',
        'Oct': '10',
        'Nov': '11',
        'Dec': '12'
    };
    return months[monthString] || monthString;
}
function validateDates() {
    const checkInValue = document.getElementById('checkIn').value;
    const checkOutValue = document.getElementById('checkOut').value;
    const maxOccupancyValue = parseInt(document.getElementById('maxiumOccupancy').value, 10);
    const roomQuantityValue = parseInt(document.getElementById('quantityRoom').value, 10);
   if (checkInValue !== undefined && checkInValue !== '' && checkInValue !== null &&
        checkOutValue !== undefined && checkOutValue !== '' && checkOutValue !== null) { 

        const [dayIn, monthInString, yearIn] = checkInValue.split('/');
        const monthIn = convertMonth(monthInString);
        const fullYearIn = yearIn.length === 2 ? `20${yearIn}` : yearIn;
        checkInDate = new Date(`${fullYearIn}-${monthIn}-${dayIn}T14:00:00`);

        if (isNaN(checkInDate.getTime())) {
            console.error("Invalid checkInDate:", checkInDate);
            return false; 
        }
    

        const [dayOut, monthOutString, yearOut] = checkOutValue.split('/');
        const monthOut = convertMonth(monthOutString);
        const fullYearOut = yearOut.length === 2 ? `20${yearOut}` : yearOut;
        checkOutDate = new Date(`${fullYearOut}-${monthOut}-${dayOut}T12:00:00`);

        if (isNaN(checkOutDate.getTime())) {
            console.error("Invalid checkOutDate:", checkOutDate);
            return false; 
        }


    if (checkOutDate <= checkInDate) {
        const minCheckOutDate = new Date(checkInDate);
        showToast("Thời gian trả phòng phải lớn hơn thời gian nhận phòng 1 ngày!");
        return false;
    }

    if (roomQuantityValue > maxOccupancyValue) {
        showToast("Số lượng người phải lớn hơn số lượng phòng!");
        return false;
    }

    document.getElementById('checkOut').setAttribute('min', formatDate(new Date(checkInDate.setDate(checkInDate.getDate() + 1))));
        return true;
    }
}

document.addEventListener('DOMContentLoaded', function () {
    const tomorrow = new Date();
    tomorrow.setDate(tomorrow.getDate() + 1); // Ngày mai

    const minCheckOutDate = new Date(tomorrow);
    minCheckOutDate.setDate(minCheckOutDate.getDate() + 1); // Ngày trả phòng tối thiểu

    document.getElementById('checkIn').value = checkIn || formatDate(tomorrow);
    document.getElementById('checkOut').value = checkOut || formatDate(minCheckOutDate); 

    $('#checkIn').flatpickr({
        dateFormat: "d/M/y", // Định dạng hiển thị
        minDate: "today",
    });

    $('#checkOut').flatpickr({
        dateFormat: "d/M/y", // Định dạng hiển thị
        minDate: new Date().fp_incr(1),
    });
});

// Show toast notifications
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
document.getElementById('checkIn').addEventListener('change', validateDates);
document.getElementById('checkOut').addEventListener('change', validateDates);
document.getElementById('maxiumOccupancy').addEventListener('change', validateDates);
document.getElementById('quantityRoom').addEventListener('change', validateDates);
document.getElementById('validateButton').addEventListener('click', function (event) {
    event.preventDefault();
    if (validateDates()) {
        saveToLocalStorage();
        document.querySelector('form').submit();
    }
});

