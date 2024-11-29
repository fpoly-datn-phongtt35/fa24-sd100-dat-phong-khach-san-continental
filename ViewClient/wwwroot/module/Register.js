
$(document).ready(function () {
    jQuery.validator.setDefaults({
        debug: true,
        success: function (label) {
            label.attr('id', 'valid');
        },
    });
    $("#myform").validate({
        rules: {
            email: {
                required: true,
                email: true
            },
            phoneNumber: {
                required: true,
                number: true
            },
            password: "required",
            confirm_password: {
                equalTo: "#password"
            }
        },
        messages: {
            userName: {
                required: "hãy nhập tài khoản...."
            },
            email: {
                required: "hãy nhập email...."
            },
            phoneNumber: {
                required: "hãy nhập số điện thoại..."
            },
            password: {
                required: "hãy nhập mật khẩu...."
            },
            confirm_password: {
                required: "hãy nhập vào mật khẩu...",
                equalTo: "mật khẩu không trùng lặp!"
            }
        }
    });

})

