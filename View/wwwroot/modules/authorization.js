$(document).ready(function ()
{
    _authorization.GetCurrentClaims();
})

var _authorization =
{
    GetCurrentClaims: function () {
        $.ajax({
            type: 'POST',
            url: '/Authorization/GetCurrentUserClaims',
            success: function (data) {
                if (data) {
                    $("#name_current_user").text(data.email);
                    sessionStorage.setItem(constant.Current_user, data)
                }
                else
                {
                    sessionStorage.setItem(constant.Current_user,"")
                }
            },
            error: function (xhr, status, error) {
                console.log("Error: " + error);
            }
        });
    },

    LogOut: function ()
    {
        $.ajax({
            type: 'POST',
            url: '/Authorization/Logout',
            success: function () {
                window.location.href = "/Authorization/LoginForm"
            },
            error: function (xhr, status, error) {
                console.log("Error: " + error);
            }
        });
    },
    Login : function()
    {
        var obj =
        {
            UserName : $("#typeEmailX").val(),
            Password: $("#typePasswordX").val()
        }

        var form = $("#form_login")
        form.validate({
            rules: {
                "email": {
                    required: true,
                },
                "password":
                {
                    required: true,
                    /*minlength: 8,
                    maxlength: 15*/
                }
            },
            messages: {
                "email": {
                    required: "Vui lòng nhập email",
                },
                "password":
                {
                    required: "Vui lòng nhập username",
                    /*minlength: "password không được ít hơn 8 kí tự",
                    maxlength: "password không được nhiều hơn 15 kí tự"*/
                },
            },

        });

        if (form.valid())
        {
            $.ajax({
                type: 'POST',
                url: '/Authorization/Login',
                data: { model: obj },
                success: function (data) {
                    if (data.success)
                    {
                        Notify.Success_Noti("Đăng nhập thành công!", 2);
                        setTimeout(function () {
                            window.location.href = "/"
                        }, 2000);
                    }
                },
                error: function (xhr, status, error) {
                    console.log("Error: " + error);
                }
            });
        }
    }
}