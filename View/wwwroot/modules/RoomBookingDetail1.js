'use strict';

$(document).ready(function () {
    function executeAction(actionUrl, roomBookingDetailId, additionalData = {}, successCallback, errorCallback) {
        $.ajax({
            url: actionUrl,
            method: 'POST',
            data: { id: roomBookingDetailId, ...additionalData },
            success: successCallback,
            error: errorCallback || function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Có lỗi xảy ra!',
                    text: 'Không thể kết nối tới server!'
                });
            }
        });
    }
    function handleCheckIn(roomBookingDetailId) {
        function executeCheckIn(forceCheckIn = false) {
            executeAction(
                '/RoomBooking/CheckIn2',
                roomBookingDetailId,
                { forceCheckIn: forceCheckIn },
                function (response) {
                    if (response.success) {
                        Swal.fire({
                            icon: 'success',
                            title: response.message
                        }).then(function () {
                            location.reload();
                        });
                    } else if (response.message.includes("sớm hơn ngày đặt") && !forceCheckIn) {
                        Swal.fire({
                            title: "Bạn đang check-in sớm hơn ngày đặt.",
                            text: "Bạn có muốn tiếp tục check-in không?",
                            icon: "warning",
                            showCancelButton: true,
                            confirmButtonColor: "#3085d6",
                            cancelButtonColor: "#d33",
                            confirmButtonText: "Xác nhận",
                            cancelButtonText: "Hủy"
                        }).then(function (result) {
                            if (result.isConfirmed) {
                                executeCheckIn(true); // Thực hiện check-in với xác nhận
                            } else if (result.isDismissed) {
                                Swal.fire({
                                    icon: 'info',
                                    title: 'Hủy bỏ check-in',
                                    text: 'Bạn đã hủy check-in thành công.'
                                });
                            }
                        });
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Có lỗi xảy ra',
                            text: response.message
                        });
                    }
                }
            );
        }

        executeCheckIn();
    }
    function handleCheckOut(roomBookingDetailId) {
        executeAction(
            '/RoomBooking/CheckOut2',
            roomBookingDetailId,
            {},
            function (response) {
                if (response.success) {
                    Swal.fire({
                        icon: 'success',
                        title: response.message
                    }).then(function () {
                        location.reload();
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Có lỗi xảy ra',
                        text: response.message
                    });
                }
            }
        );
    }
    
    $('#checkInButton').on('click', function () {
        const roomBookingDetailId = $(this).data('id');
        handleCheckIn(roomBookingDetailId);
    });
    
    $('#checkOutButton').on('click', function () {
        const roomBookingDetailId = $(this).data('id');
        handleCheckOut(roomBookingDetailId);
    });
});

$(document).ready(function () {
    function executeAction(actionUrl, data, successCallback, errorCallback) {
        $.ajax({
            url: actionUrl,
            method: 'POST',
            data: data,
            success: successCallback,
            error: errorCallback || function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Có lỗi xảy ra!',
                    text: 'Không thể kết nối tới server!'
                });
            }
        });
    }

    function handleUpdateCheckInAndCheckOut(roomBookingDetailId, checkInDateTime, checkOutDateTime, note, expenses, lstSerOrderDetail, ListDelete) {
        _Service_OrderDetail.GetListOBjService();
        executeAction(
            '/RoomBooking/UpdateCheckInAndCheckOutReality',
            {
                id: roomBookingDetailId,
                checkInTime: checkInDateTime,
                checkoutTime: checkOutDateTime,
                note: note,
                expenses: expenses,
                lstSerOrderDetail: lstServiceOrderDetail,
                ListDelete: LstDelete 
            },
            function (response) {
                if (response.success) {
                    Swal.fire({
                        icon: 'success',
                        title: response.message
                    }).then(function () {
                        location.reload();
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Có lỗi xảy ra',
                        text: response.message
                    });
                }
                listIdService = [];
                lstServiceOrderDetail = [];
                IdSerAdd = -1;
                LstDelete = [];
                LstIdSerAdd = [];
            }
        );
    }

    $('#btnUpdate').click(function () {
        const roomBookingDetailId = $(this).data('room-booking-detail-id');
        const selectedCheckInDateTime = $('#checkInRealityPicker').val();
        const selectedCheckOutDateTime = $('#checkOutRealityPicker').val();
        const note = $('#Note').val().trim();

        // Lấy giá trị phí hư tổn và loại bỏ dấu phẩy (nếu có), sau đó chuyển thành số
        const expensesInput = $('#Expenses').val().replace(/,/g, '');  // Loại bỏ dấu phẩy
        const expenses = parseFloat(expensesInput) || 0;  // Chuyển thành số thực

        if (expenses > 0 && !note) {
            Swal.fire({
                icon: 'warning',
                title: 'Thông báo',
                text: 'Vui lòng nhập ghi chú khi thêm phí hư tổn.'
            });
            return;
        }

        if (!roomBookingDetailId) {
            Swal.fire({
                icon: 'error',
                title: 'Lỗi',
                text: 'Không có Id phòng hợp lệ.'
            });
            return;
        }

        Swal.fire({
            title: 'Xác nhận cập nhật',
            text: 'Bạn có chắc chắn muốn cập nhật không?',
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Xác nhận',
            cancelButtonText: 'Hủy'
        }).then(function (result) {
            if (result.isConfirmed) {
                handleUpdateCheckInAndCheckOut(roomBookingDetailId, selectedCheckInDateTime, selectedCheckOutDateTime, 
                    note, expenses,lstServiceOrderDetail,LstDelete);
            }
        });
    });

    
    $('#checkInRealityPicker').flatpickr({
        enableTime: true,           
        noCalendar: false,            
        dateFormat: "d/m/Y h:i K",  
        time_24hr: false,            
        defaultHour: 8,            
        defaultMinute: 0, 
        minuteIncrement: 1,
        disableMobile: true, 
        static: true,
    });

    $('#checkOutRealityPicker').flatpickr({
        enableTime: true,
        noCalendar: false,
        dateFormat: "d/m/Y h:i K",
        time_24hr: false,
        defaultHour: 8,
        defaultMinute: 0,
        minuteIncrement: 1,
        disableMobile: true,
        static: true,
    });
});

function validateExpenses() {
    const expensesInput = document.getElementById('Expenses');
    let value = expensesInput.value;

    value = value.replace(/[^0-9,]/g, '');

    if (value.includes(',')) {
        value = value.replace(/,/g, '');
    }

    if (parseFloat(value) < 0) {
        value = '0';
    }

    expensesInput.value = value;
}


$(document).ready(function ()
{
    _Service_OrderDetail.LoadListSerOrderDetailRelated($("#SerDetailId").val());
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
            listIdService.push(Id);
            $.ajax({
                url: "/RoomBooking/GetServiceById",
                type: "post",
                data: { Id: Id },
                success: function (result) {
                    if (result != null) {
                        var formattedprice = parseFloat(result.price).toLocaleString('vi-VN');
                        formattedprice = formattedprice.replaceAll('.', ',');
                        $("#service-related").append(`
                        <tr class="align-items-center" id="ElementSer_`+ IdSerAdd + `">
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
                               <input id="ExtraSerPr_` + IdSerAdd + `" disabled class="form-control priceSer_extra" oninput="_Service_OrderDetail.CalculatingTotalPriceSer()" value="0" min="0" type="number">
                           </td>
                            <td scope="col">
                                <textarea id="NoteSer_`+ IdSerAdd + `" class="form-control text-start" placeholder="Thêm ghi chú"></textarea>
                            </td>
                            <td scope="col">
                                <a class="btn btn-danger" onclick="_Service_OrderDetail.RemoveServiceOut('`+ IdSerAdd + `')">Xóa</a>
                            </td>
                        </tr>
                        `);
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
    ResetIndex: function () {
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
        AmountExtraSer = global.NumberVNFormated(AmountExtraSer).toString();
        AmountPriceService = global.NumberVNFormated(AmountPriceService).toString();
        $("#total_service_price").html(AmountPriceService);
        $("#total_service_extra_price").html(AmountExtraSer);
    },
    LoadListSerOrderDetailRelated: function (Id) {
        $.ajax({
            url: "/RoomBooking/GetListServiceRelated",
            type: "post",
            data: { id: Id },
            success: function (result) {
                if (result != null) {
                    result.forEach(item => {
                        var formattedprice = parseFloat(item.amount).toLocaleString('vi-VN');
                        formattedprice = formattedprice.replaceAll('.', ',');
                        $("#service-related").append(`
                        <tr class="align-items-center" id="ElementSer_`+ item.id + `">
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

    GetListOBjService: function () {
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

