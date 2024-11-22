var constant =
{
    Current_user: "Current_user"
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