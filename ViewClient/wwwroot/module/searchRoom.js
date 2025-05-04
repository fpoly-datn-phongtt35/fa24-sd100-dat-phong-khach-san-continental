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
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();
    return `${day}/${month}/${year}`;
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
document.addEventListener("DOMContentLoaded", function () {
    const checkInPaging = document.getElementById('CheckInPaging');
    const checkOutPaging = document.getElementById('CheckOutPaging');
    const maxOccupancyPaging = document.getElementById('MaxOccupancyPaging');
    const quantityRoomPaging = document.getElementById('QuantityRoomPaging');
    const roomTypePaging = document.getElementById('roomTypePaging');
    const floorPaging = document.getElementById('floorPaging');

    const roomTypeId = sessionStorage.getItem("RoomTypeId");
    const floorId = sessionStorage.getItem("FloorId");

    if (roomTypePaging && roomTypeId) {
        roomTypePaging.value = roomTypeId;
    }
    if (floorPaging && floorId) {
        floorPaging.value = floorId;
    }
    if (checkInPaging) {
        checkInPaging.value = checkIn;
    }
    if (checkOutPaging) {
        checkOutPaging.value = checkOut;
    }
    if (maxOccupancyPaging) {
        maxOccupancyPaging.value = maxiumOccupancy;
    }
    if (quantityRoomPaging) {
        quantityRoomPaging.value = quantityRoom;
    }
});
// Gán giá trị mặc định
document.getElementById('maxiumOccupancy').value = maxiumOccupancy || 1;
document.getElementById('quantityRoom').value = quantityRoom || 1;

const today = new Date();
const formattedToday = formatDate(today);

// Khởi tạo Flatpickr
document.addEventListener('DOMContentLoaded', function () {
    const tomorrow = new Date(today);
    tomorrow.setDate(tomorrow.getDate() + 1); // Ngày mai

    const minCheckOutDate = new Date(tomorrow);
    minCheckOutDate.setDate(minCheckOutDate.getDate()); // Ngày trả phòng tối thiểu

    // Flatpickr cho checkIn
    const checkInPicker = $('#checkIn').flatpickr({
        dateFormat: "d/m/Y",
        minDate: "today",
        defaultDate: checkIn || tomorrow,
        onChange: function (selectedDates) {
            validateDates();

            const minCheckOut = new Date(selectedDates[0]);
            minCheckOut.setDate(minCheckOut.getDate() + 1);
            checkOutPicker.set('minDate', minCheckOut);

            const currentCheckOut = checkOutPicker.selectedDates[0];
            if (!currentCheckOut || currentCheckOut < minCheckOut) {
                checkOutPicker.setDate(minCheckOut);
            }
        }
    });

    // Flatpickr cho checkOut
    const checkOutPicker = $('#checkOut').flatpickr({
        dateFormat: "d/m/Y",
        minDate: minCheckOutDate,
        defaultDate: checkOut || minCheckOutDate,
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
        const [dayIn, monthIn, yearIn] = checkInValue.split('/');
        const checkInDate = new Date(`${yearIn}-${monthIn}-${dayIn}T14:00:00`);

        if (isNaN(checkInDate.getTime())) {
            return false;
        }

        const [dayOut, monthOut, yearOut] = checkOutValue.split('/');
        const checkOutDate = new Date(`${yearOut}-${monthOut}-${dayOut}T12:00:00`);

        if (isNaN(checkOutDate.getTime())) {
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

document.addEventListener('DOMContentLoaded', function () {
    // Xử lý validateButton
    const validateButton = document.getElementById('validateButton');
    validateButton.addEventListener('click', function (event) {
        event.preventDefault();
        if (validateDates()) {
            saveToLocalStorage();
            const checkIn = document.getElementById('checkIn');
            const checkOut = document.getElementById('checkOut');

            const [dayIn, monthIn, yearIn] = checkIn.value.split('/');
            checkIn.value = `${yearIn}-${monthIn}-${dayIn}T14:00:00`;

            const [dayOut, monthOut, yearOut] = checkOut.value.split('/');
            checkOut.value = `${yearOut}-${monthOut}-${dayOut}T12:00:00`;
            const searchForm = validateButton.closest('form');
            clearFilterSession();
            searchForm.submit();
        }
    });

    function clearFilterSession() {
        sessionStorage.removeItem("RoomTypeId");
        sessionStorage.removeItem("FloorId");
    }

    function submitFilterForm(form) {
        const checkInValue = document.getElementById('CheckInValue');
        const checkOutValue = document.getElementById('CheckOutValue');

        // Định dạng ngày trước khi gửi
        if (checkInValue && checkInValue.value) {
            const [dayIn, monthIn, yearIn] = checkInValue.value.split('/');
            checkInValue.value = `${yearIn}-${monthIn}-${dayIn}T14:00:00`;
        }
        if (checkOutValue && checkOutValue.value) {
            const [dayOut, monthOut, yearOut] = checkOutValue.value.split('/');
            checkOutValue.value = `${yearOut}-${monthOut}-${dayOut}T12:00:00`;
        }

        form.submit();
    }

    const filterForm = document.getElementById('filterRoomTypeFloor');
    if (filterForm) {
        filterForm.addEventListener('submit', function (event) {
            event.preventDefault();

            const isClearFilter = event.submitter && event.submitter.id === 'clearFilterButton';
            const roomTypeDropdown = document.getElementById('RoomTypeId');
            const floorDropdown = document.getElementById('FloorId');

            if (isClearFilter) {
                clearFilterSession();
                if (roomTypeDropdown) {
                    roomTypeDropdown.value = "";
                }
                if (floorDropdown) {
                    floorDropdown.value = "";
                }
                submitFilterForm(this);
            } else {
                submitFilterForm(this);
            }
        });
    } else {
        console.warn('filterRoomTypeFloor not found in the DOM');
    }

    // Xử lý dropdown RoomTypeId
    const roomTypeDropdown = document.getElementById('RoomTypeId');
    if (roomTypeDropdown) {
        roomTypeDropdown.addEventListener('change', function (event) {
            sessionStorage.setItem('RoomTypeId', this.value);
        });
    } else {
        console.warn('RoomTypeId not found in the DOM');
    }

    // Xử lý dropdown FloorId
    const floorDropdown = document.getElementById('FloorId');
    if (floorDropdown) {
        floorDropdown.addEventListener('change', function (event) {
            sessionStorage.setItem('FloorId', this.value);
        });
    } else {
        console.warn('FloorId not found in the DOM');
    }

    const roomTypeId = sessionStorage.getItem('RoomTypeId');
    const floorId = sessionStorage.getItem('FloorId');
    if (roomTypeDropdown && roomTypeId) {
        const validRoomTypeValues = Array.from(roomTypeDropdown.options).map(option => option.value);
        if (validRoomTypeValues.includes(roomTypeId)) {
            roomTypeDropdown.value = roomTypeId;
        } else {
            console.warn('RoomTypeId from sessionStorage not found in dropdown options:', roomTypeId);
        }
    }

    if (floorDropdown && floorId) {
        const validFloorValues = Array.from(floorDropdown.options).map(option => option.value);
        if (validFloorValues.includes(floorId)) {
            floorDropdown.value = floorId;
        } else {
            console.warn('FloorId from sessionStorage not found in dropdown options:', floorId);
        }
    }

    // Xử lý paginationForm
    const paginationForm = document.getElementById('paginationForm');
    if (paginationForm) {

        let pageNumberInput = document.getElementById('pageNumberHidden');
        if (!pageNumberInput) {
            pageNumberInput = document.createElement('input');
            pageNumberInput.type = 'hidden';
            pageNumberInput.name = 'PageNumber';
            pageNumberInput.id = 'pageNumberHidden';
            paginationForm.appendChild(pageNumberInput);
        }
        paginationForm.addEventListener('submit', function (event) {
            event.preventDefault();
            const CheckInPaging = document.getElementById('CheckInPaging');
            const CheckOutPaging = document.getElementById('CheckOutPaging');
            const roomTypePaging = document.getElementById('roomTypePaging');
            const floorPaging = document.getElementById('floorPaging');
            const pageSizeInput = document.getElementById('PageSize');

            if (CheckInPaging && CheckInPaging.value) {
                try {
                    const [dayIn, monthIn, yearIn] = CheckInPaging.value.split('/');
                    if (dayIn && monthIn && yearIn) {
                        CheckInPaging.value = `${yearIn}-${monthIn}-${dayIn}T14:00:00`;
                    } else {
                        console.warn('Invalid CheckInPaging format:', CheckInPaging.value);
                    }
                } catch (error) {
                    console.error('Error formatting CheckInPaging:', error);
                }
            } else {
                console.warn('CheckInPaging is empty or not found:', CheckInPaging?.value);
            }

            if (CheckOutPaging && CheckOutPaging.value) {
                try {
                    const [dayOut, monthOut, yearOut] = CheckOutPaging.value.split('/');
                    if (dayOut && monthOut && yearOut) {
                        CheckOutPaging.value = `${yearOut}-${monthOut}-${dayOut}T12:00:00`;
                    } else {
                        console.warn('Invalid CheckOutPaging format:', CheckOutPaging.value);
                    }
                } catch (error) {
                    console.error('Error formatting CheckOutPaging:', error);
                }
            } else {
                console.warn('CheckOutPaging is empty or not found:', CheckOutPaging?.value);
            }

            let pageNumber = '';
            if (event.submitter && event.submitter.name === 'PageNumber') {
                pageNumber = event.submitter.value;
                pageNumberInput.value = pageNumber;
            } else {
                console.warn('No valid submitter found for PageNumber:', event.submitter);
            }
            const pageSizeValue = pageSizeInput ? pageSizeInput.value : null;
            console.log('Form data before submit:', {
                roomTypeId: roomTypePaging?.value,
                floorId: floorPaging?.value,
                PageSize: document.getElementById('PageSize')?.value,
                PageNumber: pageNumber
            });

            event.target.submit();
        });
    } else {
        console.warn('paginationForm not found in the DOM');
    }
});