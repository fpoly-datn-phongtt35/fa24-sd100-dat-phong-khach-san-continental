﻿
$(document).ready(function () {

/*    $('.select_time"]').on('change', function ()
    {

    });*/

/*    if ($("#IdRoomBooking").val()) {
        $(".btn-luu").remove();
    }*/
    $("#RoomType_Id").select2({
        placeholder: "Chọn loại phòng",
        maximumSelectionLength: 1,
        ajax: {
            url: "/RoomBooking/GetRoomTypeSuggestion",
            type: "post",
            dataType: 'json',
            delay: 250,
            processResults: function (response) {
                return {
                    results: $.map(response, function (item) {
                        return {
                            text: item.name + ' - Sức chứa: ' + item.maximumOccupancy,
                            id: item.id,
                        }
                    })
                };
            },
        }
    }).on('select2:opening', function (e) {
        $('#RoomType_Id').val([]).trigger('change');
        getRoomRq.RoomTypeId = null;
    });

    $("#RoomType_Id").on("select2:select", function (e) {
        var data = e.params.data;
        getRoomRq.RoomTypeId = data.id;
    });


    $("#Floor_Id").select2({
        placeholder: "Chọn tầng",
        maximumSelectionLength: 1,
        ajax: {
            url: "/RoomBooking/GetFloorSuggestion",
            type: "post",
            dataType: 'json',
            delay: 250,
            processResults: function (response) {
                return {
                    results: $.map(response, function (item) {
                        return {
                            text: item.name,
                            id: item.id,
                        }
                    })
                };
            },
        }
    }).on('select2:opening', function (e) {
        $('#Floor_Id').val([]).trigger('change');
        getRoomRq.FloorId = null;
    });

    $("#Floor_Id").on("select2:select", function (e) {
        var data = e.params.data;
        getRoomRq.FloorId = data.id;
    });

    var currentTime = moment(global.formatDateToMMDDYYYY(global.createNewDateInVietnamTimezone().toISOString().slice(0, 16))).format("YYYY-MM-DD");
    $("#fromDate").attr('min', currentTime);
    $("#toDate").attr('min', currentTime);
    
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
        _Service_OrderDetail.LoadListSerOrderDetailRelated($("#IdRoomBooking").val());
    }

    $("#room_Id").select2({
        placeholder: "Tìm tên phòng",
        maximumSelectionLength: 1,
        ajax: {
            url: "/RoomBooking/GetAvailableRooms",
            type: "post",
            dataType: 'json',
            delay: 250,
            data: function (params) {
                getRoomRq.Name = params.term;
                var query = {
                    roomRequest : getRoomRq,
                }

                // Query parameters will be ?search=[term]&type=public
                return query;
            },
            processResults: function (response) {
                return {
                    results: $.map(response.lstRoom, function (item) {
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

    $("#room_Id").on("select2:select", function (e) {
        var data = e.params.data;
        _roombooking_detail.AddRoomToList(data.id)
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
                getSerRq.Name = params.term
                var query = {
                    request: getSerRq,
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

    $("#service_Id").on("select2:select", function (e) {
        var data = e.params.data;
        _Service_OrderDetail.AddServiceToList(data.id)
    });


    $("#service_type_Id").select2({
        placeholder: "Tìm tên loại dịch vụ",
        maximumSelectionLength: 1,
        ajax: {
            url: "/RoomBooking/GetlistServiceType",
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
        $('#service_type_Id').val([]).trigger('change');
        getSerRq.ServiceTypeId = null;
    });


    $("#service_type_Id").on("select2:select", function (e) {
        var data = e.params.data;
        getSerRq.ServiceTypeId = data.id;
    });
})


var getSerRq =
{
    Name: null,
    ServiceTypeId: null,
    MinPrice: null,
    MaxPrice: null
}

var getRoomRq = {
    Name : null,
    FloorId : null,
    RoomTypeId : null,
    StartDate : null,
    EndDate : null,
    MinPrice : null,
    MaxPrice : null
}

var roomBookingStatus = 1;
var RoomBooking;
var lstRoomBookingDetail = [];
var lstIdRoom = [];
var IdAdd = -1;
var LstIdAdd = [];
var LstIdUpdate = [];
var STT = 1;

var listIdService = [];
var lstServiceOrderDetail = [];
var IdSerAdd = -1;
var LstDelete = [];
var LstIdSerAdd = [];
var STT2 = 1;

var _Service_OrderDetail =
{
    LoadListRoomRelated: function (Id) {
        $.ajax({
            url: "/RoomBooking/GetListServiceRelated",
            type: "post",
            data: { id: Id },
            success: function (result) {
                $("#service-related").append();
            }
        });
    },
    OnchangeMinSer: function () {
        getSerRq.MinPrice = $("#min_Ser").val();
    },
    OnchangeMaxSer: function () {
        getSerRq.MaxPrice = $("#max_Ser").val();
    },
    AddServiceToList: function (Id) {
        if (listIdService.indexOf(Id) < 0) {
            listIdService.push(Id)
            $.ajax({
                url: "/RoomBooking/GetServiceById",
                type: "post",
                data: { Id: Id },
                success: function (result) {
                    if (result != null) {
                        var formattedprice = parseFloat(result.price).toLocaleString('vi-VN');
                        formattedprice = formattedprice.replaceAll('.', ',')
                        $("#service-related").append(`
                        <tr class="text-white align-items-center" id="ElementSer_`+ IdSerAdd + `">
                            <td style="display:none"><input id="IdSer_`+ IdSerAdd + `" value="${result.id}"></input></td>
                            <td scope="col" class="STT_Ser"></td>
                            <td scope="col">${result.name}</td>
                            <td scope="col">
                                <input id="PriceSer_`+ IdSerAdd + `" class="form-control" value="${formattedprice}" disabled />
                            </td>
                            <td scope="col">
                                <input id="QuantitySer_`+ IdSerAdd + `" onchange="_Service_OrderDetail.OnchangeQuantity('` + IdSerAdd + `')" class="form-control" type="number" value="1" min="1" />
                            </td>
                            <td scope="col">
                                ${result.unit}
                            </td>
                            <td scope="col">
                                <input id="TotalPriceSer_`+ IdSerAdd + `" class="form-control total_price_ser" value="${formattedprice}" disabled />
                            </td>
                            <td class="">
                               <input id="ExtraSerPr_` + IdSerAdd + `" class="form-control priceSer_extra" oninput="_Service_OrderDetail.CalculatingTotalPriceSer()" value="0" min="0" type="number">
                           </td>
                            <td scope="col">
                                <textarea id="NoteSer_`+ IdSerAdd + `" class="form-control text-start" placeholder="Thêm ghi chú"></textarea>
                            </td>
                            <td scope="col">
                                <a class="btn btn-danger" onclick="_Service_OrderDetail.RemoveServiceOut('`+ IdSerAdd + `')">Xóa</a>
                            </td>
                        </tr>
                        `)
                        LstIdSerAdd.push(IdSerAdd);
                        IdSerAdd = IdSerAdd - 1;
                        _Service_OrderDetail.CalculatingTotalPriceSer();
                        _Service_OrderDetail.ResetIndex();
                    }
                    
                }
            });
        }
    },
    RemoveServiceOut: function (Id) {
        if (Id < 0) {
            var Idremove = $("#IdSer_" + Id).val();
            listIdService = listIdService.filter(x => x != Idremove);
            LstIdSerAdd = LstIdSerAdd.filter(x => x != Id);
        }
        LstDelete.push(Id);
        $("#ElementSer_" + Id).remove();
        _Service_OrderDetail.CalculatingTotalPriceSer();
        _Service_OrderDetail.ResetIndex();
    },
    ResetIndex: function ()
    {
        STT2 = 1;
        var EleIndex = $('tr[id^="ElementSer_"]');
        EleIndex.each(function () {
            $(this).find('td[class^="STT_Ser"]').html(STT2);
            STT2++;
        });
    },
    OnchangeQuantity: function (Id) {
        var TotalPriceSer = $("#PriceSer_" + Id).val().replaceAll(',', '') * $("#QuantitySer_" + Id).val();
        var formattedprice = parseFloat(TotalPriceSer).toLocaleString('vi-VN');
        formattedprice = formattedprice.replaceAll('.', ',')
        $("#TotalPriceSer_" + Id).val(formattedprice);
        _Service_OrderDetail.CalculatingTotalPriceSer();
    },
    CalculatingTotalPriceSer: function () {
        var AmountPriceService = 0;
        var AmountExtraSer = 0;
        var lstToalPriceSerEle = document.getElementsByClassName("total_price_ser");
        var lstExtraSerElete = document.getElementsByClassName("priceSer_extra");
        for (let i = 0; i < lstToalPriceSerEle.length; i++) {
            const element = lstToalPriceSerEle[i];
            AmountPriceService = AmountPriceService + parseInt(element.value.replaceAll(',', ''));
        }

        for (let i = 0; i < lstExtraSerElete.length; i++) {
            const element = lstExtraSerElete[i];
            AmountExtraSer = AmountExtraSer + parseInt(element.value);
        }
        $("#total_service_price").html(global.NumberVNFormated(AmountPriceService));
        $("#total_service_extra_price").html(global.NumberVNFormated(AmountExtraSer));
        _roombooking_detail.CalculatingRoomPrice();
    },
    LoadListSerOrderDetailRelated: function (Id)
    {
        $.ajax({
            url: "/RoomBooking/GetListServiceRelated",
            type: "post",
            data: { id: Id},
            success: function (result) {
                if (result != null)
                {
                    result.forEach(item =>
                    {
                        var formattedprice = parseFloat(item.amount).toLocaleString('vi-VN');
                        formattedprice = formattedprice.replaceAll('.', ',')
                        $("#service-related").append(`
                        <tr class="text-white align-items-center" id="ElementSer_`+ item.id + `">
                            <td style="display:none"><input id="IdSerOrder_`+ item.id + `" value="${item.id}"></input></td>
                            <td style="display:none"><input id="IdSer_`+ item.id + `" value="${item.serviceId}"></input></td>
                            <td scope="col" class="STT_Ser">${STT2}</td>
                            <td scope="col">${item.name}</td>
                            <td scope="col">
                                <input id="PriceSer_`+ item.id + `" class="form-control" value="${formattedprice}" disabled />
                            </td>
                            <td scope="col">
                                <input id="QuantitySer_`+ item.id + `" onchange="_Service_OrderDetail.OnchangeQuantity('` + item.id + `')" class="form-control" type="number" value="${item.quantity}" min="1" />
                            </td>
                            <td scope="col">
                                ${item.unit}
                            </td>
                            <td scope="col">
                                <input id="TotalPriceSer_`+ item.id + `" class="form-control total_price_ser" value="${formattedprice}" disabled />
                            </td>
                            <td class="">
                               <input id="ExtraSerPr_` + item.id + `" class="form-control priceSer_extra" oninput="_Service_OrderDetail.CalculatingTotalPriceSer()" value="${item.extraPrice}" min="0" type="number">
                           </td>
                            <td scope="col">
                                <textarea id="NoteSer_`+ item.id + `" class="form-control text-start" placeholder="Thêm ghi chú">${item.description != null ? item.description : ""}</textarea>
                            </td>
                            <td scope="col">
                                <a class="btn btn-danger" onclick="_Service_OrderDetail.RemoveServiceOut('`+ item.id + `')">Xóa</a>
                            </td>
                        </tr>
                        `)
                        STT2++;
                        LstIdSerAdd.push(item.id);
                    })
                    _Service_OrderDetail.CalculatingTotalPriceSer();
                    _Service_OrderDetail.ResetIndex();
                }
            }
        });
    },

    GetListOBjService: function ()
    {
        LstIdSerAdd.forEach(item => {
            var obj =
            {
                Id: $("#IdSerOrder_" + item).val(),
                Price: $("#PriceSer_" + item).val(),
                Amount: $("#TotalPriceSer_" + item).val(),
                Quantity: $("#QuantitySer_" + item).val(),
                ExtraPrice: $("#ExtraSerPr_" + item).val(),
                Description: $("#NoteSer_" + item).val(),
                ServiceId: $("#IdSer_" + item).val()
            }
            lstServiceOrderDetail.push(obj);
        })
    }
}


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

                        <tr class="text-white">
                           <td style="display:none"><input id="Idroom_`+ IdAdd + `" value="${result.id}"></input></td>
                           <td style="display:none"><input id="Id_`+ IdAdd + `" value=""></input></td>
                           <td style="display:none"><input id="Status_`+ IdAdd + `" value="1"></input></td>
                           <td scope="row">${STT}</td>
                           <td>${result.name}</td>
                           <td class="room_price" id="Price_` + IdAdd + `">${result.price}</td>
                           <td class="deposit" id="Deposit_` + IdAdd + `"></td>
                           <td>
                               <input id="CheckIn_` + IdAdd + `" onchange="_roombooking_detail.OnchangeFromDateRow(` + IdAdd + `)" class="form-control checkin_time select_time" value="` + $("#fromDate").val() + `" type="date">
                           </td>
                           <td >
                               <input id="CheckOut_` + IdAdd + `" onchange="_roombooking_detail.OnchangeToDateRow(` + IdAdd + `)" class="form-control checkout_time select_time" value="` + $("#toDate").val() + `" type="date">
                           </td>
                           <td class="price_room" id="RoomPr_` + IdAdd + `"></td>
                           <td class="">
                               <input id="ExtraPr_` + IdAdd + `" oninput="_roombooking_detail.CalculatingRoomPrice()" class="form-control price_extra" value="0" min="0" type="number">
                           </td>
                           <td>
                               <input id="CheckInReal_`+ IdAdd + `" class="form-control" disabled value="" type="datetime-local">
                           </td>
                           <td >
                               <input id="CheckOutReal_`+ IdAdd + `" class="form-control" disabled value="" type="datetime-local">
                           </td>
                           <td id="StatusRBD_`+ IdAdd + `">Tạo mới</td>
                           <td></td>
                           <td class="d-flex">
                           </td>
                        </tr>
                        `)
                        STT++;
                        LstIdAdd.push(IdAdd);
                        $("#CheckIn_" + IdAdd).attr('min', moment(global.createNewDateInVietnamTimezone()).format("YYYY-MM-DD"));
                        $("#CheckOut_" + IdAdd).attr('min', moment(global.createNewDateInVietnamTimezone()).format("YYYY-MM-DD"));
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
        $("#CheckOut_"+Id).attr('min', $("#CheckIn_"+Id).val());
        $("#CheckIn_" + Id).val($("#CheckIn_"+Id).val());
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
        var Price = $("#Price_" + Id).text().replaceAll(',','') * daycount;
        $("#RoomPr_" + Id).html(global.NumberVNFormated(Price));
        $("#Deposit_" + Id).html(global.NumberVNFormated(Price * 0.2));
    },

    RemoveOutList: function (Id)
    {
        if (Id < 0)
        {
            var Idremove = $("#Id_" + Id).val();
            lstIdRoom = lstIdRoom.filter(x => x != Idremove);
            LstIdAdd = LstIdAdd.filter(x => x != Id);
        }
        $("#Element_" + Id).remove();
        _roombooking_detail.CalculatingRoomPrice();
    },
    OnchangeFromDate: function ()
    {
        $("#toDate").attr('min', $("#fromDate").val());
        $("#toDate").val($("#fromDate").val());
        getRoomRq.StartDate = $("#fromDate").val();
        var SelectEle = document.getElementsByClassName('select_time');
        for (let i = 0; i < SelectEle.length; i++) {
            SelectEle[i].setAttribute('min', $("#fromDate").val());
            SelectEle[i].value = $("#fromDate").val();
        }
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
        var TotalExtraPrice = 0;
        var TotalBill = 0;
        var lstEleRoomPrce = document.querySelectorAll('td[id^="RoomPr_"]');
        var lstEleDeposit = document.querySelectorAll('td[id^="Deposit_"]');
        var lstEleExtra = document.querySelectorAll('input[id^="ExtraPr_"]');

        for (let i = 0; i < lstEleExtra.length; i++) {
            const element = lstEleExtra[i];
            var idEle = element.id.replace('ExtraPr_', '');
            var statusEle = $("#Status_" + idEle).val();
            if (statusEle != 3)
            {
                TotalExtraPrice = TotalExtraPrice + parseInt(element.value);
            }
        }

        for (let i = 0; i < lstEleRoomPrce.length; i++) {
            const element = lstEleRoomPrce[i];
            var idEle = element.id.replace('RoomPr_', '');
            var statusEle = $("#Status_" + idEle).val();
            if (statusEle != 3) {
                TotalRoomPrice = TotalRoomPrice + parseInt(element.innerHTML.replaceAll(',','')); 
            }
        }

        for (let i = 0; i < lstEleDeposit.length; i++) {
            const element = lstEleDeposit[i];
            TotalDeposit = TotalDeposit + parseInt(element.innerHTML.replaceAll(',',''));
        }
        TotalBill = TotalRoomPrice + TotalExtraPrice - TotalDeposit + parseInt($("#total_service_extra_price").text().replaceAll(',', '')) + parseInt($("#total_service_price").text().replaceAll(',', ''));
        $("#total_price").text(global.NumberVNFormated(TotalBill)); 
        $("#total_roomprice").text(global.NumberVNFormated(TotalRoomPrice));
        $("#total_deposit").text(global.NumberVNFormated(TotalDeposit));
        $("#TotalExtraPrice").text(global.NumberVNFormated(TotalExtraPrice));
    },

    OnchangetoDate: function () {
        getRoomRq.EndDate = $("#toDate").val();
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
                        LstIdUpdate.push(item.roomBookingDetailId);
                        const datefrom = new Date(item.checkInBooking);
                        var newFMfrom = moment(datefrom).format("YYYY-MM-DD")
                        const dateto = new Date(item.checkOutBooking);
                        var newFMto = moment(dateto).format("YYYY-MM-DD")
                        var newCIfrom;
                        var newCOto
                        if (item.checkInReality != null)
                        {
                            const dateChein = new Date(item.checkInReality);
                            newCIfrom = dateChein.toISOString().slice(0, 16);
                        }
                        if (item.checkOutReality != null) {
                            const dateCheOut = new Date(item.checkOutReality);
                            newCOto = dateCheOut.toISOString().slice(0, 16);
                        }
                        $("#room-related").append(`
                        <tr class="text-white">
                           <td style="display:none"><input id="Idroom_`+ item.roomBookingDetailId + `" value="${item.roomId}"></input></td>
                           <td style="display:none"><input id="Id_`+ item.roomBookingDetailId + `" value="${item.roomBookingDetailId}"></input></td>
                           <td style="display:none"><input id="Status_`+ item.roomBookingDetailId + `" value="${item.status}"></input></td>
                           <td scope="row">${STT}</td>
                           <td>${item.name}</td>
                           <td class="room_price" id="Price_` + item.roomBookingDetailId + `">${global.NumberVNFormated(item.price)}</td>
                           <td class="deposit" id="Deposit_` + item.roomBookingDetailId + `">${global.NumberVNFormated(item.deposit)}</td>
                           <td>
                               <input id="CheckIn_` + item.roomBookingDetailId + `" disabled onchange="_roombooking_detail.OnchangeFromDateRow(` + item.roomBookingDetailId + `)" class="form-control checkin_time select_time" value="` + newFMfrom + `" type="date">
                           </td>
                           <td >
                               <input id="CheckOut_` + item.roomBookingDetailId + `" disabled onchange="_roombooking_detail.OnchangeToDateRow(` + item.roomBookingDetailId + `)" class="form-control checkout_time select_time" value="` + newFMto + `" type="date">
                           </td>
                           <td class="price_room" id="RoomPr_` + item.roomBookingDetailId + `"></td>
                           <td class="">
                               <input id="ExtraPr_` + item.roomBookingDetailId + `" oninput="_roombooking_detail.CalculatingRoomPrice()" class="form-control price_extra" value="` + item.extraPrice +`" min="0" type="number">
                           </td>
                           <td>
                               <input id="CheckInReal_`+ item.roomBookingDetailId + `" disabled class="form-control" value="` + newCIfrom + `" type="datetime-local">
                           </td>
                           <td >
                               <input id="CheckOutReal_`+ item.roomBookingDetailId + `" disabled class="form-control" value="` + newCOto + `" type="datetime-local">
                           </td>
                           <td id="StatusRBD_`+ item.roomBookingDetailId + `">` + global.getResponseStatus(item.status, constant.Entity_Status) + `</td>
                           <td>`+ global.getResponseStatus(item.roomStatus, constant.Room_Status) +`</td>
                           <td class="d-flex">
                               <a class="text-danger btn" id="btn-huy-` + item.roomBookingDetailId +`" style="cursor:pointer" onclick="_roombooking_detail.cancleRoomBookingDetail('`+ item.roomBookingDetailId + `','` + item.roomId +`')">Hủy</a>
                               <a class="text-green btn btn-success ms-2 btn-checkin text-nowrap" id="btn-checkin-` + item.roomBookingDetailId +`" style="cursor:pointer" onclick="_roombooking_detail.CheckIn('`+ item.roomBookingDetailId + `')">Đã nhận</a>
                               <a class="text-white btn btn-warning ms-2 btn-checkout text-nowrap" id="btn-checkout-` + item.roomBookingDetailId +`" style="cursor:pointer" onclick="_roombooking_detail.CheckOut('`+ item.roomBookingDetailId + `','` + item.roomId +`')">Đã trả</a>
                           </td>
                        </tr>
                        `)
                        if (item.status == "3" || item.status == "8")
                        {
                            $("#btn-huy-" + item.roomBookingDetailId).remove();
                            $("#btn-checkin-" + item.roomBookingDetailId).remove();
                            $("#btn-checkout-" + item.roomBookingDetailId).remove();
                            $("#ExtraPr_" + item.roomBookingDetailId).attr('disabled','disabled');
                        }
                        else if (item.status == "2")
                        {
                            $("#btn-huy-" + item.roomBookingDetailId).remove();
                            $("#btn-checkin-" + item.roomBookingDetailId).remove();
                        }
                        else if (item.status == "1") {
                            $("#btn-checkout-" + item.roomBookingDetailId).remove();
                        }
                        STT++;
                        _roombooking_detail.CalculatePrice(item.roomBookingDetailId);   
                    })
                    _roombooking_detail.CalculatingRoomPrice();
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

    cancleRoomBookingDetail: function (Id)
    {
        $("#Status_" + Id).val(3);
        $("#StatusRBD_" + Id).html("Đã hủy");
        $("#StatusRBD_" + Id).css('color', 'red');
        $("#ExtraPr_" + Id).attr('disabled', 'disabled');
        _roombooking_detail.CalculatingRoomPrice()
        $("#btn-huy-" + Id).remove();
    },

    CheckIn: function (Id)
    {
        $("#Status_" + Id).val(2);
        $("#StatusRBD_" + Id).html("Đã nhận phòng");
        var newDate = global.createNewDateInVietnamTimezone();
        newDate = newDate.toISOString().slice(0, 16);
        $("#StatusRBD_" + Id).css('color', 'yellow');
        $("#CheckInReal_" + Id).val(newDate); 
        $("#btn-checkin-" + Id).remove();
        _roombooking_detail.CalculatingRoomPrice()
    },

    CheckOut: function (Id, IdRoom) {
        $("#Status_" + Id).val(8);
        $("#StatusRBD_" + Id).html("Đã khóa");
        var newDate = global.createNewDateInVietnamTimezone();
        newDate = newDate.toISOString().slice(0, 16);
        $("#CheckOutReal_" + Id).val(newDate);
        $("#StatusRBD_" + Id).css('color', 'White');
        $("#btn-checkout-" + Id).remove();
        _roombooking_detail.CalculatingRoomPrice()
    },
    OnchangeMinRoom: function () {
        getRoomRq.MinPrice = $("#min_room").val();
    },
    OnchangeMaxRoom: function () {
        getRoomRq.MaxPrice = $("#max_room").val();
    },

    GetlistObjSubmit: function ()
    {
        RoomBooking =
        {
            Id: $("#IdRoomBooking").val(),
            CustomerId : $("#Client_Id").val(),
            Status: roomBookingStatus,
            TotalPrice: parseInt($("#total_roomprice").text().replaceAll(',', '')) + parseInt($("#total_service_price").text().replaceAll(',', '')) + parseInt($("#TotalExtraPrice").text().replaceAll(',', '')),
            TotalExtraPrice: parseInt($("#TotalExtraPrice").text().replaceAll(',', '')) + parseInt($("#total_service_extra_price").text().replaceAll(',', '')),
            TotalRoomPrice: parseInt($("#total_roomprice").text().replaceAll(',', '')),
            TotalServicePrice: parseInt($("#total_service_price").text().replaceAll(',',''))
        }

        LstIdAdd.forEach(item =>
        {
            var obj =
            {
                RoomId: $("#Idroom_" + item).val(),
                CheckInBooking: $("#CheckIn_" + item).val(),
                CheckOutBooking: $("#CheckOut_" + item).val(),
                CheckInReality: null,
                CheckOutReality: null,
                Price: $("#Price_" + item).text(),
                ExtraPrice: $("#ExtraPr_"+item).val(),
                Deposit: $("#Deposit_" + item).text(),
                Status: $("#Status_" + item).val()
            }
            lstRoomBookingDetail.push(obj);
        });
        LstIdUpdate.forEach(item =>
        {
            var obj =
            {
                Id: $("#Id_" + item).val(),
                RoomId: $("#Idroom_" + item).val(),
                CheckInBooking: $("#CheckIn_" + item).val(),
                CheckOutBooking: $("#CheckOut_" + item).val(),
                CheckInReality: $("#CheckInReal_" + item).val(),
                CheckOutReality: $("#CheckOutReal_" + item).val(),
                Price: $("#Price_" + item).text(),
                ExtraPrice: $("#ExtraPr_" + item).val(),
                Deposit: $("#Deposit_" + item).text(),
                Status: $("#Status_" + item).val()
            }
            lstRoomBookingDetail.push(obj);
        })
        console.log(RoomBooking)
        console.log(lstRoomBookingDetail)
    },

    submit: function ()
    {
        _roombooking_detail.GetlistObjSubmit();
        _Service_OrderDetail.GetListOBjService();
        $.ajax({
            url: "/RoomBooking/submit",
            type: "post",
            data: { bookingcreaterequest: RoomBooking, lstupsert: lstRoomBookingDetail, lstSerOrderDetail : lstServiceOrderDetail, ListDelete : LstDelete},
            success: function (result) {
                window.location.reload();
            }
        });
    }
}