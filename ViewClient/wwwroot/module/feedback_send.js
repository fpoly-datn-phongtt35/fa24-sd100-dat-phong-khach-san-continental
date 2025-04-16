$(document).ready(function () {
    $("#roomTable").on("click", ".btn-rating", function () {
        let clickedStar = $(this);
        let roomId = clickedStar.closest(".stars").data("room-id");
        let rating = parseInt(clickedStar.data("value"));

        // Cập nhật màu sắc cho đúng hàng sao
        let starContainer = $(`.stars[data-room-id="${roomId}"]`);
        starContainer.find(".btn-rating").each(function () {
            let starValue = parseInt($(this).data("value"));
            if (starValue <= rating) {
                $(this).addClass("selected");
            } else {
                $(this).removeClass("selected");
            }
        });

        // Lưu lại số sao đã chọn
        starContainer.attr("data-rating", rating);
    });
});

var lstId = [];
var originalFeedbacks = {}; 

var _feedback_send = {
    showPopup: function (id) {
        let popup = document.getElementById('ratingPopup');
        popup.classList.add('show');
        popup.style.display = 'flex';
        this.renderTable(id);
    },
    renderTable: function (id) {
        $.ajax({
            type: 'POST',
            url: '/RoomBooking/GetRBDWithoutComments',
            data: { id: id },
            success: function (roomsWithoutFeedback) {
                $.ajax({
                    type: 'POST',
                    url: 'https://localhost:7130/api/Feedback/GetFeedbacks',
                    data: JSON.stringify({ roomBookingId: id }),
                    contentType: 'application/json',
                    success: function (feedbackResponse) {
                        lstId = [];
                        originalFeedbacks = {};
                        let tableBody = document.getElementById('roomTable');
                        tableBody.innerHTML = ''; // Clear previous content
                        var index = 0;

                        let feedbacks = feedbackResponse.data || [];

                        // phòng có feedback
                        if (feedbacks.length > 0) {
                            feedbacks.forEach(feedback => {
                                lstId.push(feedback.roomBookingDetailId);
                                originalFeedbacks[feedback.roomBookingDetailId] = {
                                    id: feedback.id,
                                    rating: feedback.rating,
                                    comments: feedback.comments || '',
                                    customerId: feedback.customerId || ''
                                };
                                index += 1;
                                let row = `<tr class="align-items-center">
                                    <td>${index}</td>
                                    <td>${feedback.roomName}</td>
                                    <td>
                                        <div class="stars" data-room-id="${feedback.roomBookingDetailId}" data-rating="${feedback.rating}">
                                            <span class="star btn-rating ${feedback.rating >= 1 ? 'selected' : ''}" data-value="1">★</span>
                                            <span class="star btn-rating ${feedback.rating >= 2 ? 'selected' : ''}" data-value="2">★</span>
                                            <span class="star btn-rating ${feedback.rating >= 3 ? 'selected' : ''}" data-value="3">★</span>
                                            <span class="star btn-rating ${feedback.rating >= 4 ? 'selected' : ''}" data-value="4">★</span>
                                            <span class="star btn-rating ${feedback.rating >= 5 ? 'selected' : ''}" data-value="5">★</span>
                                        </div>
                                    </td>
                                    <td>
                                        <textarea class="form-control" rows="2" data-room-id="${feedback.roomBookingDetailId}">${feedback.comments || ''}</textarea>
                                    </td>
                                    <td>
                                        <button class="btn btn-sm btn-warning" onclick="_feedback_send.edit('${feedback.roomBookingDetailId}')">Sửa</button>
                                    </td>
                                </tr>`;
                                tableBody.innerHTML += row;
                            });
                        }

                        if (roomsWithoutFeedback != null && roomsWithoutFeedback.length > 0) {
                            roomsWithoutFeedback.forEach(room => {
                                if (!feedbacks.some(f => f.roomBookingDetailId === room.roomBookingDetailId)) {
                                    lstId.push(room.roomBookingDetailId);
                                    index += 1;
                                    let row = `<tr class="align-items-center">
                                        <td>${index}</td>
                                        <td>${room.name}</td>
                                        <td>
                                            <div class="stars" data-room-id="${room.roomBookingDetailId}" data-rating="0">
                                                <span class="star btn-rating" data-value="1">★</span>
                                                <span class="star btn-rating" data-value="2">★</span>
                                                <span class="star btn-rating" data-value="3">★</span>
                                                <span class="star btn-rating" data-value="4">★</span>
                                                <span class="star btn-rating" data-value="5">★</span>
                                            </div>
                                        </td>
                                        <td>
                                            <textarea class="form-control" rows="2" placeholder="Your comment" data-room-id="${room.roomBookingDetailId}"></textarea>
                                        </td>
                                        <td>
                                            <button class="btn btn-sm btn-success" onclick="_feedback_send.submit('${room.roomBookingDetailId}')">Gửi</button>
                                        </td>
                                    </tr>`;
                                    tableBody.innerHTML += row;
                                }
                            });
                        }

                        if (index === 0) {
                            tableBody.innerHTML = `<tr><td colspan="5" class="text-center">Không có phòng nào để hiển thị</td></tr>`;
                        }
                    },
                    error: function (xhr, status, error) {
                    }
                });
            },
            error: function (xhr, status, error) {
                let tableBody = document.getElementById('roomTable');
                tableBody.innerHTML = `<tr><td colspan="5" class="text-center">Không có phòng nào để hiển thị</td></tr>`;
            }
        });
    },
    closePopup: function () {
        document.getElementById('ratingPopup').classList.remove('show');
    },
    submitAllFeedback: function () {
        if (lstId.length > 0) {
            var lstObj = [];
            var updatedFeedbacks = [];
            var newlyAddedIds = []; 

            lstId.forEach(item => {
                if (!originalFeedbacks[item]) {
                    var Obj = {
                        RoomBookingDetailId: item,
                        Comments: $(`textarea[data-room-id="${item}"]`).val(),
                        Rating: $(`.stars[data-room-id="${item}"]`).attr("data-rating"),
                        Status: 1,
                        CreatedTime: null,
                        CreatedBy: null
                    };
                    if (Obj.Rating != 0) {
                        lstObj.push(Obj);
                        newlyAddedIds.push(item);  
                    }
                }
            });

            // add new
            if (lstObj.length > 0) {
                $.ajax({
                    type: 'POST',
                    url: '/Feedback/SubmitAll',
                    data: { lst: lstObj },
                    success: function () {
                        _feedback_send.closePopup();
                    },
                    error: function (xhr, status, error) {
                    }
                });
            }

            //check nếu có chỉnh sửa thì thêm vào updatedFeedbacks
            lstId.forEach(item => {
                if (originalFeedbacks[item] && !newlyAddedIds.includes(item)) {
                    let rating = $(`.stars[data-room-id="${item}"]`).attr("data-rating");
                    let comments = $(`textarea[data-room-id="${item}"]`).val();
                    let original = originalFeedbacks[item];
                    let currentRating = parseInt(rating) || original.rating;
                    let currentComments = comments || '';

                    if (currentRating !== original.rating || currentComments !== original.comments) {
                        updatedFeedbacks.push({
                            id: original.id,
                            comments: currentComments,
                            rating: currentRating,
                            status: 1,
                            deleted: false,
                            modifiedTime: new Date().toISOString(),
                            modifiedBy: original.customerId || ''
                        });
                    }
                }
            });

            //cập nhật nếu updatedFeedbacks > 0
            if (updatedFeedbacks.length > 0) {
                updatedFeedbacks.forEach(feedback => {
                    $.ajax({
                        type: 'PUT',
                        url: 'https://localhost:7130/api/Feedback/UpdateFeedback',
                        contentType: 'application/json',
                        data: JSON.stringify(feedback),
                        success: function () {
                            _feedback_send.closePopup();
                        },
                        error: function (xhr, status, error) {
                        }
                    });
                });
            }
        }
    },
    submit: function (id) {
        var lstObj = [];
        var Obj = {
            RoomBookingDetailId: id,
            Comments: $(`textarea[data-room-id="${id}"]`).val(),
            Rating: $(`.stars[data-room-id="${id}"]`).attr("data-rating"),
            Status: 1,
            CreatedTime: null,
            CreatedBy: null
        };
        if (Obj.Rating != 0) {
            lstObj.push(Obj);
            if (lstObj.length > 0) {
                $.ajax({
                    type: 'POST',
                    url: '/Feedback/SubmitAll',
                    data: { lst: lstObj },
                    success: function () {
                        _feedback_send.closePopup();
                    },
                    error: function (xhr, status, error) {
                    }
                });
            }
        }
    },
    edit: function (id) {
        let rating = $(`.stars[data-room-id="${id}"]`).attr("data-rating");
        let comments = $(`textarea[data-room-id="${id}"]`).val();
        let original = originalFeedbacks[id];

        if (original && rating && parseInt(rating) > 0) {
            let feedback = {
                id: original.id,
                comments: comments || '',
                rating: parseInt(rating),
                status: 1,
                deleted: false,
                modifiedTime: new Date().toISOString(),
                modifiedBy: original.customerId || ''
            };

            $.ajax({
                type: 'PUT',
                url: 'https://localhost:7130/api/Feedback/UpdateFeedback',
                contentType: 'application/json',
                data: JSON.stringify(feedback),
                success: function () {
                    _feedback_send.closePopup();
                },
                error: function (xhr, status, error) {
                }
            });
        }
    }
}