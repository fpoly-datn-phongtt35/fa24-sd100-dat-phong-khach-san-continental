$(document).ready(function () {
})
var _feedback_send =
{
    selectedRating: 0,
    rooms: [
        { id: 1, name: 'Room 101', rating: 0, comment: '' },
        { id: 2, name: 'Room 102', rating: 0, comment: '' },
        { id: 3, name: 'Room 103', rating: 0, comment: '' },
        { id: 4, name: 'Room 104', rating: 0, comment: '' },
        { id: 5, name: 'Room 105', rating: 0, comment: '' },
    ],
    showPopup: function () {
        let popup = document.getElementById('ratingPopup');
        popup.classList.add('show');
        popup.style.display = 'flex';
        this.renderTable();
    },
    renderTable: function () {
        let tableBody = document.getElementById('roomTable');
        tableBody.innerHTML = ''; // Clear previous content
        this.rooms.forEach((room, index) => {
            let row = `<tr>
                        <td>${index + 1}</td>
                        <td>${room.name}</td>
                        <td>
                            <div class="stars" data-room-id="${room.id}">
                                <span class="star btn-rating" data-value="1">★</span>
                                <span class="star btn-rating" data-value="2">★</span>
                                <span class="star btn-rating" data-value="3">★</span>
                                <span class="star btn-rating" data-value="4">★</span>
                                <span class="star btn-rating" data-value="5">★</span>
                            </div>
                        </td>
                        <td>
                            <textarea class="form-control" rows="2" placeholder="Your comment" data-room-id="${room.id}"></textarea>
                        </td>
                        <td>
                            <button class="btn btn-sm btn-danger" onclick="_feedback.removeRoom(${room.id})">Remove</button>
                        </td>
                    </tr>`;
        });
        this.bindStarEvents();
    },
    closePopup: function () {
        document.getElementById('ratingPopup').classList.remove('show');
    },
    submitFeedback: function () {
        let comment = document.getElementById('comment').value;
        alert(`Thank you for your feedback!\nRating: ${this.selectedRating}\nComment: ${comment}`);
        this.closePopup();
    },
    highlightStars: function (rating) {
        document.querySelectorAll('.star').forEach(star => {
            star.classList.remove('active');
            if (star.dataset.value <= rating) {
                star.classList.add('active');
            }
        });
    },
    setStars: function (rating) {
        this.selectedRating = rating;
        document.querySelectorAll('.star').forEach(star => {
            if (star.dataset.value <= rating) {
                star.classList.add('active');
            } else {
                star.classList.remove('active');
            }
        });
    }
}