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
    document.getElementById('maxiumOccupancy').value = 1; // Giá trị mặc định
}

if (quantityRoom) {
    document.getElementById('quantityRoom').value = quantityRoom;
} else {
    document.getElementById('quantityRoom').value = 1; // Giá trị mặc định
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

// Hàm xác thực ngày
function validateDates() {
    const checkInValue = document.getElementById('checkIn').value;
    const checkOutValue = document.getElementById('checkOut').value;

    const checkInDate = new Date(checkInValue);
    const checkOutDate = new Date(checkOutValue);

    const minCheckOutDate = new Date(checkInDate);
    minCheckOutDate.setDate(checkInDate.getDate() + 1); // Ngày trả phòng phải lớn hơn ngày nhận phòng ít nhất 1 ngày

    if (checkOutDate < minCheckOutDate) {
        alert("Ngày trả phòng phải lớn hơn ngày nhận phòng ít nhất 1 ngày.");
        return false;
    }

    document.getElementById('checkOut').setAttribute('min', checkInValue);
    return true;
}

// Thêm sự kiện cho các input
document.getElementById('checkIn').addEventListener('change', validateDates);
document.getElementById('checkOut').addEventListener('change', validateDates);
document.getElementById('validateButton').addEventListener('click', function (event) {
    if (validateDates()) {
        saveToLocalStorage(); // Lưu vào localStorage khi xác thực thành công
    } else {
        event.preventDefault(); // Ngăn gửi form nếu không hợp lệ
    }
});