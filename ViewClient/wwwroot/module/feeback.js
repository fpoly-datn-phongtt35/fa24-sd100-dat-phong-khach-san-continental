$(document).ready(function () {
    _feedback.LoadListFeedback();
    /*setInterval(() => {
        _feedback.LoadListFeedback();
    }, 3000);*/
})

var searchModel =
{
    Id: null,
    CustomerId: null,
    RoomId: $("#Id_Room").val(),
    Rating: null,
    Status: 1,
    FromDate: null,
    ToDate: null,
    PageIndex: 1,
    PageSize: 10
}

var _feedback =
{
    LoadListFeedback: function () {
        $.ajax({
            type: 'POST',
            url: '/Feedback/GetListFeedbacks',
            data: { request: searchModel },
            success: function (data) {
                $('#ListCMT').html('');
                $('#ListCMT').prepend(data);
            },
            error: function (xhr, status, error) {
                console.log("Error: " + error);
            }
        });
    },
    OnPanging: function (value) {
        searchModel.PageIndex = value;
        this.LoadListFeedback()
    },
    OnChangeRating: function () {
        searchModel.Rating = $("#rating").find(":selected").val();
        searchModel.PageIndex = 1;
        this.LoadListFeedback()
    },
}