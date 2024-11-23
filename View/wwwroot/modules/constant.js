var constant =
{
    Current_user: "Current_user",
    Entity_Status :
    {
        "Đã xác nhận" : 1,
        "Đã nhận phòng" : 2,
        "Đã hủy" : 3,
        "Pending" : 4,
        "PendingForActivation" : 5,
        "Chờ duyệt" : 6,
        "PendingForApproval" : 7,
        "Đã khóa" : 8,
    },
    Room_Status :
    {
        "Phòng trống" : 1,//trống
        "OutOfOrder" : 2,
        "Đã xóa" : 3,//Bị xóa
        "Đang được sử dụng" : 4,//Không trống
        "Reserved" : 5,
        "Cleaned" : 6,
        "Chưa được dọn" : 7,
        "Inspected" : 8,
        "DoNotDisturb" : 9,
        "CheckIn" : 10,
        "CheckOut" : 11,
        "Chờ duyệt" : 12
    }
}

var Notify =
{
    Success_Noti: function (Msg,second)
    {
        $('body').append(`<div class="fixed-notification alert alert-success">
        ${Msg}
        </div>`,)
        setTimeout(() => {
            $(".alert-success").remove();
        }, second * 1000);
    },
    Faild_Noti: function (Msg, second) {
        $('body').append(`<div class="fixed-notification alert alert-danger">
        ${Msg}
        </div>`,)
        setTimeout(() => {
            $(".alert-danger").remove();
        }, second * 1000);
    }
}