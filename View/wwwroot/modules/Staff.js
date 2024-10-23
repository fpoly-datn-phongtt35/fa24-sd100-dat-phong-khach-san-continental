$(document).ready(function () {
    _staff.LoadData();

    $(document.body).on('click', '.close-form', function (e)
    {
        $("#AddOrUpdateStaffForm").remove();
    })
})

let pageIndex = 1;
let pageSize = 10;
let search = '';

var _staff =
{
    Submit: function () {
        var obj =
        {
            Id : $("#Id").val(),
            FirstName : $("#firstname").val(),
            Lastname : $("#lastname").val(),
            PhoneNumber : $("#phonenumber").val(),
            Email: $("#email").val(),
            UserName: $("#username").val(),
            Password: $("#password").val(),
            RoleId: '',
            Status : 1
        }
        var form = $("#staff-form");
        form.validate({
            rules: {
                "firstname": {
                    required: true,
                },
                "lastname": {
                    required: true,
                },
                "email": {
                    required: true,
                    email: true
                },
                "phonenumber": {
                    required: true,
                    number: true
                },
                "username":
                {
                    required: true,
                    minlength: 8,
                    maxlength: 30
                },
                "password":
                {
                    required: true,
                    minlength: 8,
                    maxlength: 30
                }
            },
            messages: {
                "firstname": {
                    required: "Vui lòng nhập tên",
                },
                "lastname": {
                    required: "Vui lòng nhập họ",
                },
                "email": {
                    required: "Vui lòng nhập email",
                    email: "Email không hợp lệ"
                },
                "phonenumber": {
                    required: "Vui lòng nhập số điện số thoại",
                    number: "Số điện thoại không hợp lệ"
                },
                "username":
                {
                    required: "Vui lòng nhập username",
                    minlength: "username không được ít hơn 8 kí tự",
                    maxlength: "username không được nhiều hơn 15 kí tự"
                },
                "password":
                {
                    required: "Vui lòng nhập username",
                    minlength: "password không được ít hơn 8 kí tự",
                    maxlength: "password không được nhiều hơn 15 kí tự"
                },
            },

        });

        if (form.valid())
        {
            $.ajax({
                type: 'POST',
                url: '/Staff/UpdateOrCreate',
                data: { request : obj },
                success: function () {
                    window.location.reload();
                },
                error: function (xhr, status, error) {
                    console.log("Error: " + error);
                }
            });
        }
    },
    LoadData: function () {
        var request =
        {
            PageIndex: pageIndex,
            PageSize: pageSize,
            Search: search
        }
        $.ajax({
            type: 'POST',
            url: '/Staff/GetListData',
            data: request,
            success: function (data) {
                $('#ListStaff').html('');
                $('#ListStaff').prepend(data);
            },
            error: function (xhr, status, error) {
                console.log("Error: " + error);
            }
        });
    },
    OnPanging: function (value) {
        pageIndex = value;
        this.LoadData();
    },
    OnChangePageSize: function () {
        pageSize = $("#selectPaggingOptions").val();
        this.LoadData();
    },
    LoadForm: function () {
        $.ajax({
            type: 'POST',
            url: '/Staff/AddOrUpdateStaffForm',
            success: function (data) {
                $('body').append(data);
            },
            error: function (xhr, status, error) {
                console.log("Error: " + error);
            }
        });
    }
}