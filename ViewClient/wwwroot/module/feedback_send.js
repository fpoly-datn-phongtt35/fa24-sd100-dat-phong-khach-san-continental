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

var _feedback_send =
{
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
            success: function (data) {
                lstId = [];
                let tableBody = document.getElementById('roomTable');
                tableBody.innerHTML = ''; // Clear previous content
                var index = 0;
                if (data != null && data.length > 0) {
                    data.forEach(room => {
                        lstId.push(room.roomBookingDetailId)
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
                            <button class="btn btn-sm btn-success" onclick="_feedback_send.submit('${room.roomBookingDetailId}')">Submit</button>
                        </td>
                    </tr>`;
                        tableBody.innerHTML += row;
                    });
                }
                else {
                    tableBody.innerHTML = `<tr><td colspan="5" class="text-center">Không có phòng đủ điều kiện</td></tr>`;
                }
            },
            error: function (xhr, status, error) {
                console.log("Error: " + error);
            }
        });
    },
    closePopup: function () {
        document.getElementById('ratingPopup').classList.remove('show');
    },
    submitAllFeedback: function () {
        if (lstId.length > 0)
        {
            var lstObj = [];
            lstId.forEach(item =>
            {
                var Obj =
                {
                    RoomBookingDetailId : item,
                    Comments: $(`textarea[data-room-id="${item}"]`).val(),
                    Rating: $(`.stars[data-room-id="${item}"]`).attr("data-rating"),
                    Status: 1,
                    CreatedTime: null,
                    CreatedBy : null
                }
                if (Obj.Rating != 0)
                {
                    lstObj.push(Obj);
                }
            })
        }
        $.ajax({
            type: 'POST',
            url: '/Feedback/SubmitAll',
            data: { lst: lstObj },
            success: function () {
                _feedback_send.closePopup();
            },
            error: function (xhr, status, error) {
                console.log("Error: " + error);
            }
        });
    },
    submit: function (id) {
        var lstObj = [];

        var Obj =
        {
            RoomBookingDetailId: id,
            Comments: $(`textarea[data-room-id="${id}"]`).val(),
            Rating: $(`.stars[data-room-id="${id}"]`).attr("data-rating"),
            Status: 1,
            CreatedTime: null,
            CreatedBy: null
        }
        if (Obj.Rating != 0) {
            lstObj.push(Obj);
            if (lstObj.length > 0)
            {
                $.ajax({
                    type: 'POST',
                    url: '/Feedback/SubmitAll',
                    data: { lst: lstObj },
                    success: function () {
                        _feedback_send.closePopup();
                    },
                    error: function (xhr, status, error) {
                        console.log("Error: " + error);
                    }
                });
            }
        }
        
    },
    highlightStars: function (rating) {
        document.querySelectorAll('.star').forEach(star => {
            star.classList.remove('active');
            if (star.dataset.value <= rating) {
                star.classList.add('active');
            }
        });
    }
}