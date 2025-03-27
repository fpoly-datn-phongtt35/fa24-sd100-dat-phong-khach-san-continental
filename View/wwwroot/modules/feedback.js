$(document).ready(function () {
    _feedback.LoadListFeedback();
    setInterval(() => {
        _feedback.LoadListFeedback();
    }, 3000);

    $("#CusId").select2({
        placeholder: "Nhập Email khách hàng",
        ajax: {
            url: "/Feedback/GetListCustomers",
            type: "post",
            dataType: 'json',
            delay: 250,
            data: function (params) {
                var query = {
                    txt_search: params.term,
                }

                // Query parameters will be ?search=[term]&type=public
                return query;
            },
            processResults: function (response) {
                return {
                    results: $.map(response.data, function (item) {
                        return {
                            text: item.firstName + " " + item.lastName + ' - ' + item.email,
                            id: item.id,
                        }
                    })
                };
            },
        }
    }).on('select2:opening', function (e) {
        $('#CusId').val([]).trigger('change');
        searchModel.CustomerId = null;
        _feedback.LoadListFeedback()
    });

    $("#roomid").select2({
        placeholder: "Tìm tên phòng",
        ajax: {
            url: "/Feedback/GetListRooms",
            type: "post",
            dataType: 'json',
            delay: 250,
            data: function (params) {
                var query = {
                    txt_search: params.term,
                }

                // Query parameters will be ?search=[term]&type=public
                return query;
            },
            processResults: function (response) {
                return {
                    results: $.map(response.data, function (item) {
                        return {
                            text: item.name,
                            id: item.id,
                        }
                    })
                };
            },
        }
    }).on('select2:opening', function (e) {
        $('#roomid').val([]).trigger('change');
        searchModel.RoomId = null;
        _feedback.LoadListFeedback()
    });

    $("#rating").select2({
        placeholder :"Chọn đánh giá"
    })
    $("#status").select2({
        placeholder: "Chọn trạng thái"
    })
})

var searchModel =
{
    Id: null,
    CustomerId : null,
    RoomId: null,
    Rating: null,
    FromDate: null,
    ToDate: null,
    PageIndex: 1,
    PageSize: 10,
    Status: null
}

var _feedback =
{
    LoadListFeedback: function () {
        $.ajax({
            type: 'POST',
            url: '/Feedback/GetListFeedbacks',
            data: { request : searchModel },
            success: function (data) {
                $('#ListCMT').html('');
                $('#ListCMT').prepend(data);
            },
            error: function (xhr, status, error) {
                console.log("Error: " + error);
            }
        });
    },
    OnChangePageSize: function () {
        searchModel.PageSize = $("#selectPaggingOptions").val();
        this.LoadListFeedback()
    },
    OnPanging: function (value) {
        searchModel.PageIndex = value;
        this.LoadListFeedback()
    },
    OnChangeStatus: function () {
        searchModel.Status = $("#status").find(":selected").val();
        searchModel.PageIndex = 1;
        this.LoadListFeedback()
    },
    OnChangeRating: function () {
        searchModel.Rating = $("#rating").find(":selected").val();
        searchModel.PageIndex = 1;
        this.LoadListFeedback()
    },
    OnChangeRoom: function () {
        searchModel.RoomId = $("#roomid").val();
        searchModel.PageIndex = 1;
        this.LoadListFeedback()
    },
    OnChangeCustomer: function () {
        searchModel.CustomerId = $("#CusId").val(); 
        searchModel.PageIndex = 1;
        this.LoadListFeedback()
    },
    OnchangeFromDateRow: function () {
        /*$("#Fromdate").attr('min', this.addOrSubtractDays($("#ToDate").val(), 1));
        $("#ToDate").val($("#ToDate").val());*/
        searchModel.FromDate = $("#FromDate").val();
        searchModel.PageIndex = 1;
        this.LoadListFeedback()
    },
    addOrSubtractDays: function (dateString, days) {
        const date = new Date(dateString);
        date.setDate(date.getDate() + days);
        return date.toISOString().split('T')[0];
    },
    OnchangeToDateRow: function () {
        /*$("#Fromdate").attr('min', this.addOrSubtractDays($("#ToDate").val(), 1));
        $("#FromDate").val($("#FromDate").val());*/
        searchModel.ToDate = $("#ToDate").val(); 
        searchModel.PageIndex = 1;
        this.LoadListFeedback()
    },
    ResetConditions: function () {
        searchModel.Id = null
        searchModel.CustomerId = null
        searchModel.RoomId = null
        searchModel.Rating = null
        searchModel.FromDate = null
        searchModel.ToDate = null
        searchModel.PageIndex = 1
        searchModel.PageSize = 10
        searchModel.Status = null

        $('#roomid').val([]).trigger('change');
        $('#CusId').val([]).trigger('change');
        $('#rating').val([]).trigger('change');
        $('#status').val([]).trigger('change');
        document.getElementById("FromDate").value = "";
        document.getElementById("ToDate").value = "";
        
        

        this.LoadListFeedback()
    },
    OnchangeVisibled: async function (status,id)
    {
        const confrm = await global.Noti("Xác nhận cập nhật", "Bạn có chắc chắn muốn cập nhật không?");
        if (confrm > 0)
        {
            $.ajax({
                type: 'POST',
                url: '/Feedback/UpdateVisibled',
                data: { status: status, id : id },
                success: function (data) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Thông báo cập nhật',
                        text: 'Cập nhật thành công'
                    });
                    location.reload();
                },
                error: function (xhr, status, error) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Cập nhật thất bại',
                        text: 'Vui lòng liên hệ ban IT để được hỗ trợ.'
                    });
                }
            });
        }
    }
}