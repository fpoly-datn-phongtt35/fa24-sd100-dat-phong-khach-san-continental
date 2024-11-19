$(document).ready(function () {

/*    $('.select_time"]').on('change', function ()
    {

    });*/

    if (!$("#IdClient").val()) {
        $("#Client_Id").select2({
            placeholder: "Tìm theo UserName, Email, SĐT",
            maximumSelectionLength: 1,
            ajax: {
                url: "/RoomBooking/GetCustomerSuggestion",
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
                                text: item.userName + ' - ' + item.email,
                                id: item.id,
                            }
                        })
                    };
                },
            }
        }).on('select2:opening', function (e) {
            $('#Client_Id').val([]).trigger('change');
            $("#FirstName").val('');
            $("#LastName").val('');
            $("#BirthDay").val('');
            $("#Gender").val('');
            $("#Email").val('');
            $("#Phone").val('');
        });
        $("#Client_Id").on("select2:select", function (e) {
            var data = e.params.data;
            _roombooking_detail.LoadDetailCustomer(data.id);
        });
    }
    else
    {
        $("#Client_Id").select2();
        _roombooking_detail.LoadDetailCustomer($("#Client_Id").val());
        _roombooking_detail.LoadListRoomRelated($("#IdRoomBooking").val());
    }

    $("#room_Id").select2({
        placeholder: "Tìm tên phòng",
        maximumSelectionLength: 1,
        ajax: {
            url: "/RoomBooking/GetRoomSuggestion",
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
                            text: item.name + ' - ' + item.description,
                            id: item.id,
                        }
                    })
                };
            },
        }
    }).on('select2:opening', function (e) {
        $('#room_Id').val([]).trigger('change');
    });

    $("#service_Id").select2({
        placeholder: "Tìm tên dịch vụ",
        maximumSelectionLength: 1,
        ajax: {
            url: "/RoomBooking/GetServiceSuggestion",
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
                    results: $.map(response, function (item) {
                        return {
                            text: item.name + ' - ' + item.description,
                            id: item.id,
                        }
                    })
                };
            },
        }
    }).on('select2:opening', function (e) {
        $('#service_Id').val([]).trigger('change');
    });

    $("#room_Id").on("select2:select", function (e) {
        var data = e.params.data;
        _roombooking_detail.AddRoomToList(data.id)
    });
})


var roomBookingStatus = 1;

var lstRoomBookingDetail = [];
var lstIdRoom = [];
var IdAdd = -1;
var LstIdAdd = [];
var STT = 1;

var listIdService = [];
var IdSerAdd = -2;
var STT2 = 1;

var _roombooking_detail = {
    AddRoomToList: function (Id) {
        if (lstIdRoom.indexOf(Id) < 0)
        {
            lstIdRoom.push(Id);
            $.ajax({
                url: "/RoomBooking/GetRoomById",
                type: "post",
                data: { Id: Id },
                success: function (result) {
                    if (result != null) {
                        $("#room-related").append(`
                        <tr class="text-white" Id="Element_` +IdAdd+ `">
                           <th style="display:none"><input id="Id_`+ IdAdd + `" value="${result.id}"></input></th>
                           <th style="display:none"><input id="Status_`+ IdAdd + `" value="1"></input></th>
                           <th scope="row">${STT}</th>
                           <td>${result.name}</td>
                           <td class="room_price" id="Price_` + IdAdd +`">${result.price}</td>
                           <td class="deposit" id="Deposit_` + IdAdd + `"></td>
                           <td>
                               <input id="CheckIn_` + IdAdd + `" onchange="_roombooking_detail.OnchangeFromDateRow(` + IdAdd + `)" class="form-control checkin_time select_time" value="` + $("#fromDate").val() + `" type="datetime-local">
                           </td>
                           <td >
                               <input id="CheckOut_` + IdAdd + `" onchange="_roombooking_detail.OnchangeToDateRow(` + IdAdd + `)" class="form-control checkout_time select_time" value="` + $("#toDate").val() + `" type="datetime-local">
                          </td>
                           <td class="price_room" id="RoomPr_` + IdAdd + `"></td>
                           <td id="CheckInReal_`+ IdAdd +`"></td>
                           <td id="CheckOutReal_`+ IdAdd +`"></td>
                           <td>
                               <a class="text-warning" style="cursor:pointer" onclick="_roombooking_detail.RemoveOutList(`+ IdAdd +`)">Delete</a>
                           </td>
                        </tr>
                        `)
                        STT++;
                        LstIdAdd.push(IdAdd);
                        _roombooking_detail.CalculatePrice(IdAdd);                        
                        IdAdd = IdAdd - 1;
                    }
                    _roombooking_detail.CalculatingRoomPrice();
                }
            });
        }
    },

    OnchangeFromDateRow: function (Id)
    {
        _roombooking_detail.CalculatePrice(Id);
        _roombooking_detail.CalculatingRoomPrice();
    },
    OnchangeToDateRow: function (Id) {
        _roombooking_detail.CalculatePrice(Id);
        _roombooking_detail.CalculatingRoomPrice();
    },

    calculateDaysDifference : function(startDate, endDate) {
        // Chuyển đổi các chuỗi ngày thành đối tượng Date
        const date1 = new Date(startDate);
        const date2 = new Date(endDate);

        // Tính toán số mili giây giữa hai ngày
        const differenceInMilliseconds = Math.abs(date2 - date1);

        // Chuyển đổi số mili giây thành số ngày
        const differenceInDays = Math.ceil(differenceInMilliseconds / (1000 * 3600 * 24));

        return differenceInDays;
    },

    CalculatePrice: function (Id)
    {
        var fromdate = $("#CheckIn_" + Id).val();
        var todate = $("#CheckOut_" + Id).val();
        var daycount = _roombooking_detail.calculateDaysDifference(fromdate, todate);
        var Price = $("#Price_" + Id).text() * daycount;
        $("#RoomPr_" + Id).html(Price);
        $("#Deposit_" + Id).html(Price * 0.2);
    },

    RemoveOutList: function (Id)
    {
        var Idremove = $("#Id_" + Id).val();
        lstIdRoom = lstIdRoom.filter(x => x != Idremove);
        LstIdAdd = LstIdAdd.filter(x => x != Id);
        $("#Element_" + Id).remove();
        _roombooking_detail.CalculatingRoomPrice();
    },
    OnchangeFromDate: function ()
    {
        var lstEleCheckIn = document.getElementsByClassName('checkin_time');
        for (let i = 0; i < lstEleCheckIn.length; i++) {
            const element = lstEleCheckIn[i];
            element.value = $("#fromDate").val();
        }
        LstIdAdd.forEach(item =>
        {
            _roombooking_detail.CalculatePrice(item);
        })
        _roombooking_detail.CalculatingRoomPrice();
    },

    CalculatingRoomPrice: function ()
    {
        var TotalRoomPrice = 0;
        var TotalDeposit = 0;
        var lstEleRoomPrce = document.getElementsByClassName('price_room');
        var lstEleDeposit = document.getElementsByClassName('deposit');
        for (let i = 0; i < lstEleRoomPrce.length; i++) {
            const element = lstEleRoomPrce[i];
            TotalRoomPrice = TotalRoomPrice + parseInt(element.innerHTML); 
        }

        for (let i = 0; i < lstEleDeposit.length; i++) {
            const element = lstEleDeposit[i];
            TotalDeposit = TotalDeposit + parseInt(element.innerHTML);
        }

        $("#total_roomprice").text(TotalRoomPrice);
        $("#total_deposit").text(TotalDeposit);
    },

    OnchangetoDate: function () {
        var lstEleCheckIn = document.getElementsByClassName('checkout_time');
        for (let i = 0; i < lstEleCheckIn.length; i++) {
            const element = lstEleCheckIn[i];
            element.value = $("#toDate").val();
        }
        LstIdAdd.forEach(item => {
            _roombooking_detail.CalculatePrice(item);
        })
        _roombooking_detail.CalculatingRoomPrice();
    },

    LoadListRoomRelated: function (Id)
    {
        $.ajax({
            url: "/RoomBooking/GetRoomRelated",
            type: "post",
            data: { Id: Id },
            success: function (result) {
                if (result != null) {
                    result.forEach(item =>
                    {
                        $("#room-related").append(`
                        <tr class="text-white">
                           <th scope="row">${STT}</th>
                           <td>${item.name}</td>
                           <td>${item.price}</td>
                           <td></td>
                           <td>
                               ${item.checkInBooking}
                           </td>
                           <td>${item.checkOutBooking}</td>
                           <td>${item.checkInReality}</td>
                           <td>${item.checkOutReality}</td>
                           <td>
                               <a>Đã nhận phòng</a>
                           </td>
                        </tr>
                        `)
                        STT++;
                    })
                   
                }
            }
        });
    },
    LoadDetailCustomer: function (Id) {
        $.ajax({
            url: "/RoomBooking/GetCustomerById",
            type: "post",
            data: { Id: Id },
            success: function (result) {
                if (result != null) {
                    $("#FirstName").val(result.firstName);
                    $("#LastName").val(result.lastName);
                    $("#BirthDay").val(new Date(result.dateOfBirth).toLocaleDateString('vi-VN'));
                    if (result.gender) {
                        $("#Gender").val("Nam");
                    }
                    else {
                        $("#Gender").val("Nữ");
                    }
                    $("#Email").val(result.email);
                    $("#Phone").val(result.phoneNumber);
                }
            }
        });
    },
    GetlistObjSubmit: function ()
    {
        var RoomBooking =
        {
            Id: $("#IdRoomBooking").val(),
            CustomerId : $("#Client_Id").val(),
            Status: roomBookingStatus,
            TotalPrice: parseInt($("#total_roomprice").text()) + parseInt($("#total_service_price").text()),
            TotalRoomPrice: parseInt($("#total_roomprice").text()),
            TotalServicePrice: parseInt($("#total_service_price").text())
        }

        LstIdAdd.forEach(item =>
        {
            var obj =
            {
                RoomIds: $("#Id_" + item).val(),
                CheckInBooking: $("#CheckIn_" + item).val(),
                CheckOutBooking: $("#CheckOut_" + item).val(),
                CheckInReality: null,
                CheckOutReality: null,
                Price: $("#Price_" + item).text(),
                Deposit: $("#Deposit_" + item).text(),
                Status: $("#Status_" + item).text()
            }
            lstRoomBookingDetail.push(obj);
        });
        console.log(RoomBooking)
        console.log(lstRoomBookingDetail)
    },

    submit: function ()
    {
        _roombooking_detail.GetlistObjSubmit();
    }
}