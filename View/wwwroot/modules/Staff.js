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
            Id: $("#Id").val(),
            FirstName: $("#firstname").val(),
            Lastname: $("#lastname").val(),
            PhoneNumber: $("#phonenumber").val(),
            Email: $("#email").val(),
            UserName: $("#email").val(),
            Password: $("#password").val(),
            RoleId: '',
            Status: 1
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
                 "password":
                 {
                     required: "Vui lòng nhập password",
                     minlength: "password không được ít hơn 8 kí tự",
                     maxlength: "password không được nhiều hơn 15 kí tự"
                 },
            },

        });

        if (form.valid()) {
            $.ajax({
                type: 'POST',
                url: '/Staff/UpdateOrCreate',
                data: { request: obj },
                success: function (data) {
                    if (data.status == 200) {
                        Notify.Success_Noti(data.msg, 1.5);
                        setTimeout(function () {
                            location.reload();
                        }, 1500);
                    }
                },
                error: function (xhr, status, error) {
                    Notify.Faild_Noti(data.msg, 1.5);
                    console.log("Error: " + error);
                }
            });
        }
    },
    LoadData: function (pageIndex, pageSize, search) {
        $('#ListStaff').html(`     
        <div class="d-flex justify-content-center pt-5">
        <div id="loading-spinner" style="width: 5rem; height: 5rem;" class="spinner-border text-success pt-5" role="status">
            <span class="visually-hidden">Loading...</span>
        </div>
        </div>`);
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
    Delete: function (id) {
        $.ajax({
            type: 'POST',
            url: '/Staff/Delete',
            data: { id: id },
            success: function (data) {
                setTimeout(function () {
                    location.reload();
                }, 1500);
            },
            error: function (xhr, status, error) {
                console.log("Error: " + error);
            }
        });
    },
    OnchangeSearchText: function ()
    {
        PageIndex = 1;
        PageSize = 10;
        search = $("#txt_search").val();
        this.LoadData(1, 10, search);
    },
    OnPanging: function (value) {
        pageIndex = value;
        this.LoadData(pageIndex, pageSize, search);
    },
    OnChangePageSize: function () {
        pageSize = $("#selectPaggingOptions").val();
        this.LoadData(pageIndex, pageSize, search);
    },
    LoadForm: function (Id) {
        $.ajax({
            type: 'POST',
            url: '/Staff/AddOrUpdateStaffForm',
            data: {Id : Id},
            success: function (data) {
                $('body').append(data);
            },
            error: function (xhr, status, error) {
                console.log("Error: " + error);
            }
        });
    }
}