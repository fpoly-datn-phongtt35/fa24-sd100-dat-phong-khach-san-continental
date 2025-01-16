$(document).ready(function () {

    _roomBooking.LoadListRoomBooking();

    $("#Staff_Id").select2({
        placeholder: "Tìm tên nhân viên",
        maximumSelectionLength: 1,
        ajax: {
            url: "/Staff/GetListStaff",
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
        $('#Staff_Id').val([]).trigger('change');
        searchModel.StaffId = null;
    });

    setInterval(() => {
/*        _roomBooking.CheckDepositRoomBooking()*/
        _roomBooking.LoadListRoomBooking();
    }, 3000);
})

var searchModel =
{
    SearchString: null,
    Status: null,
    StaffId: null,
    Status2: document.querySelector('input[name="inlineRadioOptions"]:checked').value,
    PageIndex: 1,
    PageSize: 10
}

var _roomBooking =
{
    handleRadioChange: function (value)
    {
        searchModel.Status2 = value;
        searchModel.PageIndex = 1;
        this.LoadListRoomBooking(searchModel)
    },
    RedirecDetail: function (IdRoomBooking,IdUser)
    {
        location.href = "/BookingRoom/Id=" + IdRoomBooking + "&&Client=" + IdUser; 
    },
    OnInputTxtSearch: function ()
    {
        searchModel.SearchString = $("#txtSearch").val();
        searchModel.PageIndex = 1;
        this.LoadListRoomBooking(searchModel)
    },
    OnChangeStatus: function ()
    {
        searchModel.Status = $("#status").find(":selected").val();
        searchModel.PageIndex = 1;
        this.LoadListRoomBooking(searchModel)
    },
    OnChangeStaff: function ()
    {
        searchModel.StaffId = $("#Staff_Id").val();
        searchModel.PageIndex = 1;
        this.LoadListRoomBooking(searchModel)
    },
    OnChangePageSize: function ()
    {
        searchModel.PageSize = $("#selectPaggingOptions").val();
        this.LoadListRoomBooking(searchModel)
    },
    OnPanging: function (value)
    {
        searchModel.PageIndex = value;
        this.LoadListRoomBooking(searchModel)
    },
    LoadListRoomBooking: function ()
    {
        $.ajax({
            type: 'POST',
            url: '/RoomBooking/ListRoomBooking',
            data: searchModel,
            success: function (data) {
                $('#ListRoomBooking').html('');
                $('#ListRoomBooking').prepend(data);
            },
            error: function (xhr, status, error) {
                console.log("Error: " + error);
            }
        });
    }
}