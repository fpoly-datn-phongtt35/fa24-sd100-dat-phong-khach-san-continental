//#region $(document).ready
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
    else {
        $("#Client_Id").select2();
        _roombooking_detail.LoadDetailCustomer($("#Client_Id").val());
        _roombooking_detail.LoadListRoomRelated($("#IdRoomBooking").val());
        if ($('#RoomBookingStatus').val() == 2 || $('#RoomBookingStatus').val() == 3 || $('#RoomBookingStatus').val() == 4)
        {
            setInterval(() => {
                _roombooking_detail.LoadListRoomRelated($("#IdRoomBooking").val());
            }, 1000);
        }
        $("#fromDate").attr('disabled', 'disabled');
        $("#toDate").attr('disabled', 'disabled');
    }

    $("#room_Id").select2({
        placeholder: "Chọn phòng",
        maximumSelectionLength: 1,
        ajax: {
            url: "/RoomBooking/GetAvailableRooms",
            type: "post",
            dataType: 'json',
            delay: 250,
            data: function (params) {
                getRoomRq.Name = params.term;
                var query = {
                    roomRequest: getRoomRq,
                }

                // Query parameters will be ?search=[term]&type=public
                return query;
            },
            processResults: function (response) {
                $("#room_num").text('Tổng số phòng trống: ' + response.totalRoom);
                $("#ocupan").text('Tổng sức chứa: ' +response.totalOccupancy);
                return {
                    results: $.map(response.lstRoom, function (item) {
                        return {
                            text: 'phòng ' + item.name + ' - giá: ' + global.NumberVNFormated(item.price) ,
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



   
})
//#endregion

var getRoomRq = {
    Name: null,
    FloorId: null,
    RoomTypeId: null,
    StartDate: null,
    EndDate: null,
    MinPrice: null,
    MaxPrice: null
}

var roomBookingStatus = 1;
var RoomBooking;
var lstRoomBookingDetail = [];
var lstIdRoom = [];
var IdAdd = -1;
var LstIdAdd = [];
var LstIdUpdate = [];
var STT = 1;
let dataRessidence = [];
var residenceCount = 0;
var maximumOccupancy = 0;


var _roombooking_detail = {
    AddRoomToList: function (Id) {
        if (lstIdRoom.indexOf(Id) < 0) {
            lstIdRoom.push(Id);
            $("#fromDate").attr('disabled', 'disabled');
            $("#toDate").attr('disabled', 'disabled');
            $.ajax({
                url: "/RoomBooking/GetRoomById",
                type: "post",
                data: { Id: Id },
                success: function (result) {
                    if (result != null) {
                        $("#room-related").append(`

                        <tr id="Element_` + IdAdd + `" class="">
                           <td style="display:none"><input id="Idroom_`+ IdAdd + `" value="${result.id}"></input></td>
                           <td style="display:none"><input id="Id_`+ IdAdd + `" value="`+Id+`"></input></td>
                           <td style="display:none"><input id="Status_`+ IdAdd + `" value="1"></input></td>
                           <td scope="row">${STT}</td>
                           <td>${result.name}</td>
                           <td class="room_price" id="Price_` + IdAdd + `">${result.price}</td>
                           <td>
                               <input id="CheckIn_` + IdAdd + `" onchange="_roombooking_detail.OnchangeFromDateRow(` + IdAdd + `)" class="form-control checkin_time select_time" value="` + $("#fromDate").val() + `" type="date">
                           </td>
                           <td >
                               <input id="CheckOut_` + IdAdd + `" onchange="_roombooking_detail.OnchangeToDateRow(` + IdAdd + `)" class="form-control checkout_time select_time" value="` + $("#toDate").val() + `" type="date">
                           </td>
                           <td class="price_room" id="RoomPr_` + IdAdd + `"></td>
                           <td>
                               <input id="CheckInReal_`+ IdAdd + `" class="form-control" disabled value="" type="datetime-local">
                           </td>
                           <td >
                               <input id="CheckOutReal_`+ IdAdd + `" class="form-control" disabled value="" type="datetime-local">
                           </td>
                           <td id="StatusRBD_`+ IdAdd + `">Tạo mới</td>
                           <td class="d-flex">
                               <button class="btn btn-danger" onclick="_roombooking_detail.RemoveOutList(` + IdAdd +`)">Xóa</button>
                           </td>
                        </tr>
                        `)
                        STT++;
                        LstIdAdd.push(IdAdd);
                        $("#CheckIn_" + IdAdd).attr('min', moment(global.createNewDateInVietnamTimezone()).format("YYYY-MM-DD"));
                        $("#CheckOut_" + IdAdd).attr('min', moment(global.createNewDateInVietnamTimezone()).format("YYYY-MM-DD"));
                        _roombooking_detail.CalculatePrice(IdAdd);
                        IdAdd = IdAdd - 1;
                        var lstEleCheckIn = document.getElementsByClassName('checkout_time');
                        for (let i = 0; i < lstEleCheckIn.length; i++) {
                            const element = lstEleCheckIn[i];
                            element.max = $("#toDate").val();
                        }
                        var lstEleCheckIn = document.getElementsByClassName('checkin_time');
                        for (let i = 0; i < lstEleCheckIn.length; i++) {
                            const element = lstEleCheckIn[i];
                            element.max = $("#toDate").val();
                        }
                    }
                    _roombooking_detail.CalculatingRoomPrice();
                }
            });
        }
    },
    addOrSubtractDays:function(dateString, days) {
        const date = new Date(dateString);
        date.setDate(date.getDate() + days);
        return date.toISOString().split('T')[0];
    },

    OnchangeFromDateRow: function (Id) {
        $("#CheckOut_" + Id).attr('min', _roombooking_detail.addOrSubtractDays($("#CheckIn_" + Id).val(),1));
        $("#CheckIn_" + Id).val($("#CheckIn_" + Id).val());
        _roombooking_detail.CalculatePrice(Id);
        _roombooking_detail.CalculatingRoomPrice();
    },

    OnchangeToDateRow: function (Id) {
        $("#CheckIn_" + Id).attr('max', _roombooking_detail.addOrSubtractDays($("#CheckOut_" + Id).val(),-1));
        $("#CheckOut_" + Id).val($("#CheckOut_" + Id).val());
        _roombooking_detail.CalculatePrice(Id);
        _roombooking_detail.CalculatingRoomPrice();
    },

    calculateDaysDifference: function (startDate, endDate) {
        // Chuyển đổi các chuỗi ngày thành đối tượng Date
        const date1 = new Date(startDate);
        const date2 = new Date(endDate);

        // Tính toán số mili giây giữa hai ngày
        const differenceInMilliseconds = Math.abs(date2 - date1);

        // Chuyển đổi số mili giây thành số ngày
        const differenceInDays = Math.ceil(differenceInMilliseconds / (1000 * 3600 * 24));

        return differenceInDays;
    },

    CalculatePrice: function (Id) {
        var fromdate = $("#CheckIn_" + Id).val();
        var todate = $("#CheckOut_" + Id).val();
        var daycount = _roombooking_detail.calculateDaysDifference(fromdate, todate);
        var Price = $("#Price_" + Id).text().replaceAll(',', '') * daycount;
        $("#RoomPr_" + Id).html(global.NumberVNFormated(Price));
        $("#Deposit_" + Id).html(global.NumberVNFormated(Price * 0.2));
    },

    RemoveOutList: function (Id) {
        if (Id < 0) {
            var Idremove = $("#Id_" + Id).val();
            lstIdRoom = lstIdRoom.filter(x => x != Idremove);
            LstIdAdd = LstIdAdd.filter(x => x != Id);
        }
        $("#Element_" + Id).remove();
        _roombooking_detail.CalculatingRoomPrice();
    },

    OnchangeExTra: function (id,Code) {
        var ExTrapriceEle = $('input#' + Code + id);
        ExTrapriceEle.prop('disabled', 'true')
        var ExTraprice = ExTrapriceEle.val().replaceAll(',', '');
        if (ExTraprice == null || ExTraprice == '' || ExTraprice == undefined)
        {
            ExTrapriceEle.val(0);
            _roombooking_detail.CalculatingRoomPrice()
            ExTrapriceEle.prop('disabled', false)
            ExTrapriceEle.focus();
        }
        else if (/^\d+$/.test(ExTraprice)) {
            var formattedExtraprice = parseFloat(ExTraprice).toLocaleString('vi-VN');
            formattedExtraprice = formattedExtraprice.replaceAll('.', ',');
            ExTrapriceEle.val(formattedExtraprice);

            _roombooking_detail.CalculatingRoomPrice()

            ExTrapriceEle.prop('disabled', false)
            ExTrapriceEle.focus();
        }
        else {
            ExTraprice = ExTrapriceEle.val().replace(/[^\d]/g, "")
            var formattedExtraprice = parseFloat(ExTraprice).toLocaleString('vi-VN');
            formattedExtraprice = formattedExtraprice.replaceAll('.', ',')
            ExTrapriceEle.val(formattedExtraprice)
            _roombooking_detail.CalculatingRoomPrice()
            ExTrapriceEle.prop('disabled', false)
            ExTrapriceEle.focus();
        }
    },

    OnchangeFromDate: function () {
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
        LstIdAdd.forEach(item => {
            _roombooking_detail.CalculatePrice(item);
        })
        _roombooking_detail.CalculatingRoomPrice();
    },

    CalculatingRoomPrice: function () {
        var TotalRoomPrice = 0;
        var TotalDeposit = 0;
        var TotalExtraPrice = 0;
        var TotalExpense = 0;
        var TotalBill = 0;
        var lstEleRoomPrce = document.querySelectorAll('td[id^="RoomPr_"]');
        var lstEleDeposit = document.querySelectorAll('td[id^="Deposit_"]');
        var lstEleExtra = document.querySelectorAll('input[id^="ExtraPr_"]');
        var lstEleExpense = document.querySelectorAll('input[id^="Expenses_"]');

        for (let i = 0; i < lstEleExpense.length; i++) {
            const element = lstEleExpense[i];
            var idEle = element.id.replace('Expenses_', '');
            var statusEle = $("#Status_" + idEle).val();
            if (statusEle != 3) {
                TotalExpense = TotalExpense + parseInt(element.value.replaceAll(',', ''));
            }
        }

        for (let i = 0; i < lstEleExtra.length; i++) {
            const element = lstEleExtra[i];
            var idEle = element.id.replace('ExtraPr_', '');
            var statusEle = $("#Status_" + idEle).val();
            if (statusEle != 3) {
                TotalExtraPrice = TotalExtraPrice + parseInt(element.value.replaceAll(',',''));
            }
        }

        for (let i = 0; i < lstEleRoomPrce.length; i++) {
            const element = lstEleRoomPrce[i];
            var idEle = element.id.replace('RoomPr_', '');
            var statusEle = $("#Status_" + idEle).val();
            if (statusEle != 3) {
                TotalRoomPrice = TotalRoomPrice + parseInt(element.innerHTML.replaceAll(',', ''));
            }
        }

        for (let i = 0; i < lstEleDeposit.length; i++) {
            const element = lstEleDeposit[i];
            TotalDeposit = TotalDeposit + parseInt(element.innerHTML.replaceAll(',', ''));
        }
        TotalBill = TotalRoomPrice + TotalExpense + TotalExtraPrice + parseInt($("#total_service_extra_price").text().replaceAll(',', '')) + parseInt($("#total_service_price").text().replaceAll(',', ''));
        $("#total_price").text(global.NumberVNFormated(TotalBill));
        $("#total_roomprice").text(global.NumberVNFormated(TotalRoomPrice));
        $("#total_deposit").text(global.NumberVNFormated(TotalDeposit));
        $("#TotalExtraPrice").text(global.NumberVNFormated(TotalExtraPrice));
        $("#TotalExpense").text(global.NumberVNFormated(TotalExpense));
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

    LoadListRoomRelated: function (Id) {
        $.ajax({
            url: "/RoomBooking/GetRoomRelated",
            type: "post",
            data: { Id: Id },
            success: function (result) {
                if (result != null) {
                    STT = 1;
                    $("#room-related").html(``);
                    result.forEach(item => {
                        LstIdUpdate.push(item.roomBookingDetailId);
                        const datefrom = new Date(item.checkInBooking);
                        var newFMfrom = moment(datefrom).format("YYYY-MM-DD")
                        const dateto = new Date(item.checkOutBooking);
                        var newFMto = moment(dateto).format("YYYY-MM-DD")

                        var UnnePrice = item.expenses + item.extraPrice + item.extraService + item.servicePrice;
                        var Price = (item.price) / _roombooking_detail.calculateDaysDifference(item.checkInBooking, item.checkOutBooking);

                        var newCIfrom;
                        var newCOto
                        /*if (item.checkInReality != null) {
                            const dateChein = new Date(item.checkInReality);
                            newCIfrom = dateChein.toISOString().slice(0, 16);

                            Price = (item.price) / _roombooking_detail.calculateDaysDifference(item.checkInReality, item.checkOutBooking);
                        }
                        if (item.checkOutReality != null) {
                            const dateCheOut = new Date(item.checkOutReality);
                            newCOto = dateCheOut.toISOString().slice(0, 16);

                            Price = (item.price) / _roombooking_detail.calculateDaysDifference(item.checkInBooking, item.checkOutReality) ;
                        }

                        if (item.checkInReality != null && item.checkOutReality != null)
                        {
                            Price = (item.price) / _roombooking_detail.calculateDaysDifference(item.checkInReality, item.checkOutReality);
                        }*/
                        $("#room-related").append(`
                        <tr class="">
                           <td style="display:none"><input id="Idroom_`+ item.roomBookingDetailId + `" value="${item.roomId}"></input></td>
                           <td style="display:none"><input id="Id_`+ item.roomBookingDetailId + `" value="${item.roomBookingDetailId}"></input></td>
                           <td style="display:none"><input id="Status_`+ item.roomBookingDetailId + `" value="${item.status}"></input></td>
                           <td scope="row">${STT}</td>
                           <td>${item.name}</td>
                           <td class="room_price" id="Price_` + item.roomBookingDetailId + `">${global.NumberVNFormated(Price)}</td>
                           <td>
                               <input id="CheckIn_` + item.roomBookingDetailId + `" disabled onchange="_roombooking_detail.OnchangeFromDateRow(` + item.roomBookingDetailId + `)" class="form-control checkin_time select_time" value="` + newFMfrom + `" type="date">
                           </td>
                           <td >
                               <input id="CheckOut_` + item.roomBookingDetailId + `" disabled onchange="_roombooking_detail.OnchangeToDateRow(` + item.roomBookingDetailId + `)" class="form-control checkout_time select_time" value="` + newFMto + `" type="date">
                           </td>
                           <td class="price_room" id="RoomPr_` + item.roomBookingDetailId + `"></td>
                           <td>
                               <input id="CheckInReal_`+ item.roomBookingDetailId + `" disabled class="form-control" value="` + newCIfrom + `" type="datetime-local">
                           </td>
                           <td>
                               <input id="CheckOutReal_`+ item.roomBookingDetailId + `" disabled class="form-control" value="` + newCOto + `" type="datetime-local">
                           </td>
                           <td id="StatusRBD_`+ item.roomBookingDetailId + `">` + global.getResponseStatus(item.status, constant.Entity_Status) + `</td>
                           <td class="d-flex">
                               <a class="text-danger btn" id="btn-huy-` + item.roomBookingDetailId + `" style="cursor:pointer" onclick="_roombooking_detail.cancleRoomBookingDetail('` + item.roomBookingDetailId + `','` + item.roomId + `')">Hủy</a>
                               <a class="btn btn-info ms-2 btn-residence text-nowrap text-light" id="btn-residence-` + item.roomBookingDetailId + `" style="cursor:pointer" onclick="_roombooking_detail.getResidenceRegistrations('` + item.roomBookingDetailId + `')">Tạm trú</a>
                               <a class="btn btn-primary ms-2 btn-detail text-nowrap" id="btn-detail-` + item.roomBookingDetailId + `" href="/RoomBooking/RoomBookingDetails/RoomBookingDetailId=` + item.roomBookingDetailId + `" style="cursor:pointer">Chi tiết</a>
                           </td>
                           
                        </tr>
                        `)
                        if (item.status == "3" || item.status == "8") {
                            $("#btn-huy-" + item.roomBookingDetailId).remove();
                            $("#btn-checkin-" + item.roomBookingDetailId).remove();
                            $("#btn-checkout-" + item.roomBookingDetailId).remove();
                            $("#ExtraPr_" + item.roomBookingDetailId).attr('disabled', 'disabled');
                        }
                        else if (item.status == "2") {
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


    showResidenceModal: function (data, roomBookingDetailId) {
        //#region modal
        const existingModal = document.getElementById('customModal');
        if (existingModal) existingModal.remove();

        // Tạo modal container
        const modalContainer = document.createElement('div');
        modalContainer.id = 'customModal';
        modalContainer.style.position = 'fixed';
        modalContainer.style.top = '50%';
        modalContainer.style.left = '50%';
        modalContainer.style.transform = 'translate(-50%, -50%)';
        modalContainer.style.width = '80%';
        modalContainer.style.maxWidth = '1000px';
        modalContainer.style.backgroundColor = 'white';
        modalContainer.style.boxShadow = '0 4px 8px rgba(0, 0, 0, 0.2)';
        modalContainer.style.borderRadius = '8px';
        modalContainer.style.padding = '16px';
        modalContainer.style.zIndex = '1000';

        // Giới hạn chiều cao của modal
        modalContainer.style.maxHeight = '80vh';
        modalContainer.style.overflowY = 'auto';
        
        // Tạo header
        const modalHeader = document.createElement('div');
        modalHeader.style.display = 'flex';
        modalHeader.style.justifyContent = 'space-between';
        modalHeader.style.alignItems = 'center';
        modalHeader.style.marginBottom = '16px';

        const title = document.createElement('h3');
        title.innerText = 'Danh sách khách hàng ' + residenceCount + '/' + maximumOccupancy;
        modalHeader.appendChild(title);

        //#region validate residence
        function isPhoneNumberValid(phoneNumber) {
            const phoneRegex = /^(0\d{9})$/;
            return phoneNumber === "" || phoneRegex.test(phoneNumber);
        }//bắt đầu = 0, + 9 số đằng sau
        function isIdentityNumberValid(identityNumber) {
            const identityRegex = /^\d{12}$/;
            return identityNumber === "" || identityRegex.test(identityNumber);
        }// 12 số
        function isFullNameValid(name) {
            const nameRegex = /^[A-ZÁÀẢÃẠĂẮẰẲẴẠÂẦẤẨẪẬÔỒỔỖỘƠỚỜỞỠỢÍÌỈĨỊÚÙỦŨỤƠỚỜỞỠỢÉÈẺẼẸÔỒỔỖỘƠỚỜỞỠỢÝỲỶỸỴa-záàảãạăắằẳẵặâầấẩẫậôồổỗộơớờởỡợíìỉĩịúùủũụơớờởỡợéèẻẽẹôồổỗộơớờởỡợýỳỷỹỵ\s]+$/i;
            return nameRegex.test(name) && name.trim().split(/\s+/).length >= 2;
        }// có ít nhất 2 chữ, và ko đc có số
        function isPhoneNumberExisted(phoneNumber) {
            return dataRessidence.some(item => item.phoneNumber === phoneNumber);
        }
        function isIdentityNumberExisted(identityNumber) {
            return dataRessidence.some(item => item.identityNumber === identityNumber);
        }
        function isDateOfBirthValid(dateOfBirth) {
            const today = new Date();
            const birthDate = new Date(dateOfBirth);

            // Kiểm tra nếu ngày sinh hợp lệ (theo chuẩn yyyy-mm-dd)
            if (isNaN(birthDate.getTime())) {
                return false; // Ngày sinh không hợp lệ
            }

            // Kiểm tra nếu ngày sinh trống
            if (!dateOfBirth) {
                return false; // Nếu ngày sinh trống
            }

            // Kiểm tra ngày sinh không lớn hơn ngày hiện tại
            if (birthDate > today) {
                return false; // Ngày sinh không được lớn hơn ngày hiện tại
            }

            // Tính tuổi
            var age = today.getFullYear() - birthDate.getFullYear();
            var monthDiff = today.getMonth() - birthDate.getMonth();
            if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate())) {
                age--;
            }

            // Kiểm tra tuổi từ 0 đến 150
            return age <= 150;
        }
        //#endregion

        // Tạo nút "Thêm"
        const addButton = document.createElement('button');
        addButton.innerText = 'Thêm';
        addButton.id = 'residenceAddButton';
        addButton.style.backgroundColor = '#008CBA';
        addButton.style.color = 'white';
        addButton.style.padding = '8px 16px';
        addButton.style.marginRight = '8px';
        addButton.style.border = 'none';
        addButton.style.borderRadius = '4px';
        addButton.style.cursor = 'pointer';
        addButton.style.fontSize = '14px';
        addButton.style.transition = 'background-color 0.3s';
        //#endregion

        addButton.addEventListener('mouseover', function () {
            addButton.style.backgroundColor = '#007bb5';
        });
        addButton.addEventListener('mouseout', function () {
            addButton.style.backgroundColor = '#008CBA';
        });
        //addButton.onclick = function () {
        //    showAddForm();
        //};
        modalHeader.appendChild(addButton);

        modalContainer.appendChild(modalHeader);  



        //#region thêm

        addButton.addEventListener('click', function () {
            const status = $(`#Status_${roomBookingDetailId}`).val(); 
            if (status === "8" || status === "3") { 
                alert('Phòng này đã bị hủy hoặc hoàn thành. Không thể thực hiện thêm tạm trú.');
                return; 
            }
            //#region tạo khung form thêm mới
            const addForm = document.createElement('div');
            addForm.style.position = 'fixed';
            addForm.style.top = '50%';
            addForm.style.left = '50%';
            addForm.style.transform = 'translate(-50%, -50%)';
            addForm.style.width = '1000px';
            addForm.style.backgroundColor = 'white';
            addForm.style.padding = '20px';
            addForm.style.boxShadow = '0 4px 8px rgba(0, 0, 0, 0.2)';
            addForm.style.borderRadius = '8px';
            addForm.style.zIndex = '1001';
            //#endregion

            // Form thêm mới
            addForm.innerHTML = `
                <h3 style="text-align: center; font-size: 20px; font-weight: 600;">Thêm mới thông tin cư trú</h3>
                <input type="hidden" id="roomBookingDetailId" value="${roomBookingDetailId}" />
                <div style="margin-bottom: 16px;">
                    <label for="fullName" style="display: block; font-weight: 600; font-size: 14px; color: #333; margin-bottom: 8px;">Tên:</label>
                    <span id="fullNameError" style="color: red; font-size: 12px; display: none;"></span>
                    <input type="text" id="fullName" style="width: 100%; padding: 10px; border: 1px solid #ddd; border-radius: 4px; font-size: 14px; color: #333; outline: none; transition: border-color 0.3s;" />
                </div>

                <div style="margin-bottom: 16px;">
                    <label for="dateOfBirth" style="display: block; font-weight: 600; font-size: 14px; color: #333; margin-bottom: 8px;">Ngày sinh:</label>
                    <span id="dateOfBithError" style="color: red; font-size: 12px; display: none;"></span>
                    <input type="date" id="dateOfBirth" style="width: 100%; padding: 10px; border: 1px solid #ddd; border-radius: 4px; font-size: 14px; color: #333; outline: none; transition: border-color 0.3s;" />
                </div>

                <div style="margin-bottom: 16px;">
                    <label for="gender" style="display: block; font-weight: 600; font-size: 14px; color: #333; margin-bottom: 8px;">Giới tính:</label>
                    <select id="gender" style="width: 100%; padding: 10px; border: 1px solid #ddd; border-radius: 4px; font-size: 14px; color: #333; outline: none; transition: border-color 0.3s;">
                        <option value="1">Nam</option>
                        <option value="2">Nữ</option>
                        <option value="0">Khác</option>
                    </select>
                </div>

                <div style="margin-bottom: 16px;">
                    <label for="identityNumber" style="display: block; font-weight: 600; font-size: 14px; color: #333; margin-bottom: 8px;">Căn cước:</label>
                    <span id="identityNumberError" style="color: red; font-size: 12px; display: none;"></span>
                    <input type="number" id="identityNumber" style="width: 100%; padding: 10px; border: 1px solid #ddd; border-radius: 4px; font-size: 14px; color: #333; outline: none; transition: border-color 0.3s;" />
                </div>

                <div style="margin-bottom: 16px;">
                    <label for="phoneNumber" style="display: block; font-weight: 600; font-size: 14px; color: #333; margin-bottom: 8px;">Số điện thoại:</label>
                    <span id="phoneNumberError" style="color: red; font-size: 12px; display: none;"></span>
                    <input type="number" id="phoneNumber" style="width: 100%; padding: 10px; border: 1px solid #ddd; border-radius: 4px; font-size: 14px; color: #333; outline: none; transition: border-color 0.3s;" />
                </div>

                <div style="display: flex; justify-content: space-between;">
                    <button id="addSaveButton" style="background-color: #008CBA; color: white; padding: 8px 16px; border: none; border-radius: 4px; cursor: pointer; font-size: 14px; transition: background-color 0.3s;">Thêm</button>
                    <button id="addCancelButton" style="background-color: #f44336; color: white; padding: 8px 16px; border: none; border-radius: 4px; cursor: pointer; font-size: 14px; transition: background-color 0.3s;">Hủy</button>
                </div>
            `;

            // Thêm form vào body
            document.body.appendChild(addForm);

            //#region kiểm tra định đạng
            const addSaveButton = document.getElementById('addSaveButton');
            // số điện thoại
            const phoneNumberInput = document.getElementById('phoneNumber');
            var tempPhoneNumber = phoneNumberInput.value;
            const phoneNumberError = document.getElementById('phoneNumberError');
            phoneNumberInput.addEventListener('input', function () {
                if (!isPhoneNumberValid(phoneNumberInput.value)) {
                    phoneNumberError.innerText = 'Sai định dạng số điện thoại!';
                    phoneNumberError.style.display = 'block';
                    addSaveButton.disabled = true;
                } else if (isPhoneNumberExisted(phoneNumberInput.value)) {
                    phoneNumberError.innerText = ' Số điện thoại đã tồn tại!';
                    phoneNumberError.style.display = 'block';
                    addSaveButton.disabled = true;
                }
                else {
                    phoneNumberError.style.display = 'none';
                    addSaveButton.disabled = false;
                }
            });
            // số căn cước
            const identityNumberInput = document.getElementById('identityNumber');
            var tempIdentityNumber = identityNumberInput.value;
            const identityNumberError = document.getElementById('identityNumberError');
            identityNumberInput.addEventListener('input', function () {
                if (!isIdentityNumberValid(identityNumberInput.value)) {
                    identityNumberError.innerText = 'Sai định dạng số căn cước!';
                    identityNumberError.style.display = 'block';
                    addSaveButton.disabled = true;
                } else if (isIdentityNumberExisted(identityNumberInput.value)) {
                    identityNumberError.innerText = 'Số căn cước đã tồn tại!';
                    identityNumberError.style.display = 'block';
                    addSaveButton.disabled = true;
                } else {
                    identityNumberError.style.display = 'none';
                    addSaveButton.disabled = false;
                }
            });
            // fullName
            const fullNameInput = document.getElementById('fullName');
            const fullNameError = document.getElementById('fullNameError');
            fullNameInput.addEventListener('input', function () {
                if (!isFullNameValid(fullNameInput.value)) {
                    fullNameError.style.display = 'block';
                    addSaveButton.disabled = true;
                } else {
                    fullNameError.style.display = 'none';
                    addSaveButton.disabled = false;
                }
            });
            // DoB
            const dateOfBirthInput = document.getElementById('dateOfBirth');
            const dateOfBithError = document.getElementById('dateOfBithError');
            dateOfBirthInput.addEventListener('change', function () {
                if (!isDateOfBirthValid(dateOfBirthInput.value)) {
                    dateOfBithError.style.display = 'block';
                    addSaveButton.disabled = true;
                } else {
                    dateOfBithError.style.display = 'none';
                    addSaveButton.disabled = false;
                }
            });
            //#endregion

            // Xử lý sự kiện Thêm
            addSaveButton.addEventListener('click', function () {
                const dateOfBirthValue = document.getElementById('dateOfBirth').value;  
                const dateOfBirthFormatted = dateOfBirthValue ? dateOfBirthValue + 'T00:00:00' : null; 

                const newResidence = {
                    fullName: document.getElementById('fullName').value,
                    dateOfBirth: dateOfBirthFormatted,
                    gender: document.getElementById('gender').value,
                    identityNumber: document.getElementById('identityNumber').value,
                    phoneNumber: document.getElementById('phoneNumber').value,
                    roomBookingDetailId: roomBookingDetailId,
                    isCheckOut: false, 
                    checkOutTime: null  
                };

                $.ajax({
                    url: '/ResidenceRegistration/AddResidenceRegistration',
                    type: 'POST',
                    
                    data: newResidence,
                    success: function (response) {
                        if (response === 1) {
                            alert('Thêm mới thành công');
                            console.log(response)
                            const newData = {
                                id: newResidence.id, 
                                roomBookingDetailId: newResidence.roomBookingDetailId, 
                                fullName: newResidence.fullName,  
                                dateOfBirth: newResidence.dateOfBirth, 
                                gender: parseInt(newResidence.gender),  
                                identityNumber: newResidence.identityNumber, 
                                phoneNumber: newResidence.phoneNumber, 
                                isCheckOut: false,
                                checkOutTime: null  
                            };

                            data.push(newData);
                            console.log(data);

                            tbody.innerHTML = '';
                            data.forEach(item => {
                                const row = document.createElement('tr');
                                const gender = item.gender === 1 ? 'Nam' : item.gender === 2 ? 'Nữ' : 'Khác';
                                const cells = [
                                    item.fullName,
                                    moment(item.dateOfBirth).format("YYYY-MM-DD"),
                                    gender,
                                    item.identityNumber,
                                    item.phoneNumber,
                                    item.checkOutTime ? moment(item.checkOutTime).format("YYYY-MM-DD HH:mm") : 'Chưa checkout'
                                ];

                                cells.forEach(cellText => {
                                    const td = document.createElement('td');
                                    td.innerText = cellText;
                                    td.style.border = '1px solid #ddd';
                                    td.style.padding = '8px';
                                    row.appendChild(td);
                                });

                                //#region Tạo cột Actions
                                const actionsTd = document.createElement('td');
                                actionsTd.style.border = '1px solid #ddd';
                                actionsTd.style.padding = '8px';

                                const editButton = document.createElement('button');
                                editButton.innerText = 'Sửa';
                                editButton.style.backgroundColor = '#4CAF50';
                                editButton.style.color = 'white';
                                editButton.style.padding = '8px 16px';
                                editButton.style.marginRight = '8px';
                                editButton.style.border = 'none';
                                editButton.style.borderRadius = '4px';
                                editButton.style.cursor = 'pointer';
                                editButton.style.fontSize = '14px';
                                editButton.style.transition = 'background-color 0.3s';

                                const deleteButton = document.createElement('button');
                                deleteButton.innerText = 'Xóa';
                                deleteButton.style.backgroundColor = '#f44336';
                                deleteButton.style.color = 'white';
                                deleteButton.style.padding = '8px 16px';
                                deleteButton.style.marginRight = '8px';
                                deleteButton.style.border = 'none';
                                deleteButton.style.borderRadius = '4px';
                                deleteButton.style.cursor = 'pointer';
                                deleteButton.style.fontSize = '14px';
                                deleteButton.style.transition = 'background-color 0.3s';

                                const leaveButton = document.createElement('button');
                                leaveButton.innerText = 'Rời';
                                leaveButton.style.backgroundColor = '#FF9800';
                                leaveButton.style.color = 'white';
                                leaveButton.style.padding = '8px 16px';
                                leaveButton.style.marginRight = '8px';
                                leaveButton.style.border = 'none';
                                leaveButton.style.borderRadius = '4px';
                                leaveButton.style.cursor = 'pointer';
                                leaveButton.style.fontSize = '14px';
                                leaveButton.style.transition = 'background-color 0.3s';

                                if (item.checkOutTime) {
                                    leaveButton.disabled = true;
                                    leaveButton.style.backgroundColor = '#D3D3D3';
                                    leaveButton.style.cursor = 'default';


                                    deleteButton.disabled = true;
                                    deleteButton.style.backgroundColor = '#D3D3D3';
                                    deleteButton.style.cursor = 'default';


                                    editButton.disabled = true;
                                    editButton.style.backgroundColor = '#D3D3D3';
                                    editButton.style.cursor = 'default';

                                }


                                actionsTd.appendChild(editButton);
                                actionsTd.appendChild(deleteButton);
                                actionsTd.appendChild(leaveButton);
                                row.appendChild(actionsTd);

                                tbody.appendChild(row);
                                //#endregion
                                var temp = dataRessidence.length;
                                title.innerText = 'Danh sách khách hàng ' + temp + '/' + maximumOccupancy;
                            });
                            document.body.removeChild(addForm);
                        } else {
                            alert('Thêm mới thất bại');
                        }
                    },
                    error: function () {
                        alert('Có lỗi xảy ra');
                    }
                });
            });

            // Xử lý sự kiện Hủy
            const addCancelButton = document.getElementById('addCancelButton');
            addCancelButton.addEventListener('click', function () {
                document.body.removeChild(addForm);
            });
        });
        //#endregion

        //#region Tạo bảng để thêm vào modal
        const table = document.createElement('table');
        table.style.width = '100%';
        table.style.borderCollapse = 'collapse';

        const thead = document.createElement('thead');
        const headerRow = document.createElement('tr');
        const headers = ['Tên', 'Ngày sinh', 'Giới tính', 'Căn cước', 'SDT', 'Thời gian checkout', 'Hành động'];

        headers.forEach(headerText => {
            const th = document.createElement('th');
            th.innerText = headerText;
            th.style.border = '1px solid #ddd';
            th.style.padding = '8px';
            th.style.backgroundColor = '#f2f2f2';
            th.style.textAlign = 'left';
            headerRow.appendChild(th);
        });

        thead.appendChild(headerRow);
        table.appendChild(thead);

        // nội dung bảng
        const tbody = document.createElement('tbody');
        data.forEach(item => {
            const row = document.createElement('tr');

            const gender = item.gender === 1 ? 'Nam' : item.gender === 2 ? 'Nữ' : 'Khác';       

            const cells = [
                item.fullName,
                moment(item.dateOfBirth).format("YYYY-MM-DD"),
                gender,
                item.identityNumber,
                item.phoneNumber,
                item.checkOutTime ? moment(item.checkOutTime).format("YYYY-MM-DD HH:mm") : 'Chưa checkout'  
            ];

            cells.forEach(cellText => {
                const td = document.createElement('td');
                td.innerText = cellText;
                td.style.border = '1px solid #ddd';
                td.style.padding = '8px';
                row.appendChild(td);
            });

            // cột Actions
            const actionsTd = document.createElement('td');
            actionsTd.style.border = '1px solid #ddd';
            actionsTd.style.padding = '8px';

            //#region Sửa
            //#region css button sửa
            const editButton = document.createElement('button');
            editButton.innerText = 'Sửa';
            editButton.style.backgroundColor = '#4CAF50';
            editButton.style.color = 'white';
            editButton.style.padding = '8px 16px';
            editButton.style.marginRight = '8px';
            editButton.style.border = 'none';
            editButton.style.borderRadius = '4px';
            editButton.style.cursor = 'pointer';
            editButton.style.fontSize = '14px';
            editButton.style.transition = 'background-color 0.3s';
            editButton.setAttribute('data-id', item.id);
            //#endregion

            editButton.addEventListener('click', function () {
                const id = editButton.getAttribute('data-id');
                const row = editButton.closest('tr');

                const fullName = row.cells[0].innerText;
                const dateOfBirth = row.cells[1].innerText;
                const gender = row.cells[2].innerText;
                const identityNumber = row.cells[3].innerText;
                const phoneNumber = row.cells[4].innerText;
                const checkOutTime = row.cells[5].innerText;

                //#region tạo khung form chỉnh sửa
                const editForm = document.createElement('div');
                editForm.style.position = 'fixed';
                editForm.style.top = '50%';
                editForm.style.left = '50%';
                editForm.style.transform = 'translate(-50%, -50%)';
                editForm.style.width = '1000px';
                editForm.style.backgroundColor = 'white';
                editForm.style.padding = '20px';
                editForm.style.boxShadow = '0 4px 8px rgba(0, 0, 0, 0.2)';
                editForm.style.borderRadius = '8px';
                editForm.style.zIndex = '1001';
                //#endregion

                // form chỉnh sửa
                editForm.innerHTML = `
                    <h3 style="text-align: center; font-size: 20px; font-weight: 600;">Sửa thông tin cư trú</h3>
                    <div style="margin-bottom: 16px;">
                        <label for="fullName" style="display: block; font-weight: 600; font-size: 14px; color: #333; margin-bottom: 8px;">Tên:</label>
                        <span id="fullNameError" style="color: red; font-size: 12px; display: none;"></span>
                        <input type="text" id="fullName" value="${fullName}" style="width: 100%; padding: 10px; border: 1px solid #ddd; border-radius: 4px; font-size: 14px; color: #333; outline: none; transition: border-color 0.3s;" />
                    </div>

                    <div style="margin-bottom: 16px;">
                        <label for="dateOfBirth" style="display: block; font-weight: 600; font-size: 14px; color: #333; margin-bottom: 8px;">Ngày sinh:</label>
                        <span id="dateOfBithError" style="color: red; font-size: 12px; display: none;"></span>
                        <input type="date" id="dateOfBirth" value="${dateOfBirth}" style="width: 100%; padding: 10px; border: 1px solid #ddd; border-radius: 4px; font-size: 14px; color: #333; outline: none; transition: border-color 0.3s;" />
                    </div>

                    <div style="margin-bottom: 16px;">
                        <label for="gender" style="display: block; font-weight: 600; font-size: 14px; color: #333; margin-bottom: 8px;">Giới tính:</label>
                        <select id="gender" style="width: 100%; padding: 10px; border: 1px solid #ddd; border-radius: 4px; font-size: 14px; color: #333; outline: none; transition: border-color 0.3s;">
                            <option value="1" ${gender === 'Nam' ? 'selected' : ''}>Nam</option>
                            <option value="2" ${gender === 'Nữ' ? 'selected' : ''}>Nữ</option>
                            <option value="0" ${gender === 'Khác' ? 'selected' : ''}>Khác</option>
                        </select>
                    </div>

                    <div style="margin-bottom: 16px;">
                        <label for="identityNumber" style="display: block; font-weight: 600; font-size: 14px; color: #333; margin-bottom: 8px;">Căn cước:</label>
                        <span id="identityNumberError" style="color: red; font-size: 12px; display: none;"></span>
                        <input type="number" id="identityNumber" value="${identityNumber}" style="width: 100%; padding: 10px; border: 1px solid #ddd; border-radius: 4px; font-size: 14px; color: #333; outline: none; transition: border-color 0.3s;" />
                    </div>

                    <div style="margin-bottom: 16px;">
                        <label for="phoneNumber" style="display: block; font-weight: 600; font-size: 14px; color: #333; margin-bottom: 8px;">Số điện thoại:</label>
                        <span id="phoneNumberError" style="color: red; font-size: 12px; display: none;"></span>
                        <input type="number" id="phoneNumber" value="${phoneNumber}" style="width: 100%; padding: 10px; border: 1px solid #ddd; border-radius: 4px; font-size: 14px; color: #333; outline: none; transition: border-color 0.3s;" />
                    </div>

                    <div style="margin-bottom: 16px;">
                        <label for="checkOutTime" style="display: block; font-weight: 600; font-size: 14px; color: #333; margin-bottom: 8px;">Thời gian Check-Out:</label>
                        <p id="checkOutTime" style="padding: 10px; border: 1px solid #ddd; border-radius: 4px; font-size: 14px; color: #333; background-color: #f9f9f9; margin: 0;">${checkOutTime}</p>
                    </div>


                    <div style="display: flex; justify-content: space-between;">
                        <button id="saveButton" style="background-color: #008CBA; color: white; padding: 8px 16px; border: none; border-radius: 4px; cursor: pointer; font-size: 14px; transition: background-color 0.3s;">Lưu</button>
                        <button id="cancelButton" style="background-color: #f44336; color: white; padding: 8px 16px; border: none; border-radius: 4px; cursor: pointer; font-size: 14px; transition: background-color 0.3s;">Hủy</button>
                    </div>
                `;


                // thêm form vào body
                document.body.appendChild(editForm);


                //#region kiểm tra định đạng
                const saveButton = document.getElementById('saveButton');
                // số điện thoại
                const phoneNumberInput = document.getElementById('phoneNumber');
                var tempPhoneNumber = phoneNumberInput.value;
                const phoneNumberError = document.getElementById('phoneNumberError');
                phoneNumberInput.addEventListener('input', function () {
                    if (!isPhoneNumberValid(phoneNumberInput.value)) {
                        phoneNumberError.innerText = 'Sai định dạng số điện thoại!';
                        phoneNumberError.style.display = 'block';
                        saveButton.disabled = true;
                    } else if (phoneNumberInput.value != tempPhoneNumber && isPhoneNumberExisted(phoneNumberInput.value)) {
                        phoneNumberError.innerText = ' Số điện thoại đã tồn tại!';
                        phoneNumberError.style.display = 'block';
                        saveButton.disabled = true;
                    }
                    else {
                        phoneNumberError.style.display = 'none';
                        saveButton.disabled = false;
                    }
                });
                // số căn cước
                const identityNumberInput = document.getElementById('identityNumber');
                var tempIdentityNumber = identityNumberInput.value;
                const identityNumberError = document.getElementById('identityNumberError');
                identityNumberInput.addEventListener('input', function () {
                    if (!isIdentityNumberValid(identityNumberInput.value)) {
                        identityNumberError.innerText = 'Sai định dạng số căn cước!';
                        identityNumberError.style.display = 'block';
                        saveButton.disabled = true;
                    } else if (identityNumberInput.value != tempIdentityNumber && isIdentityNumberExisted(identityNumberInput.value)) {
                        identityNumberError.innerText = 'Số căn cước đã tồn tại!';
                        identityNumberError.style.display = 'block';
                        saveButton.disabled = true;
                    } else {
                        identityNumberError.style.display = 'none';
                        saveButton.disabled = false;
                    }
                });
                // fullName
                const fullNameInput = document.getElementById('fullName');
                const fullNameError = document.getElementById('fullNameError');
                fullNameInput.addEventListener('input', function () {
                    if (!isFullNameValid(fullNameInput.value)) {
                        fullNameError.style.display = 'block';
                        saveButton.disabled = true;
                    } else {
                        fullNameError.style.display = 'none';
                        saveButton.disabled = false;
                    }
                }); 
                // DoB
                const dateOfBirthInput = document.getElementById('dateOfBirth');
                const dateOfBithError = document.getElementById('dateOfBithError');
                dateOfBirthInput.addEventListener('change', function () {
                    if (!isDateOfBirthValid(dateOfBirthInput.value)) {
                        dateOfBithError.style.display = 'block';
                        saveButton.disabled = true;
                    } else {
                        dateOfBithError.style.display = 'none';
                        saveButton.disabled = false;
                    }
                });
                //#endregion

                document.getElementById('saveButton').addEventListener('click', function () {
                    const updatedResidence = {
                        id: id,
                        fullName: document.getElementById('fullName').value,
                        dateOfBirth: document.getElementById('dateOfBirth').value,
                        gender: document.getElementById('gender').value,
                        identityNumber: document.getElementById('identityNumber').value,
                        phoneNumber: document.getElementById('phoneNumber').value
                    };

                    $.ajax({
                        url: '/ResidenceRegistration/EditResidenceRegistration',
                        type: 'POST',
                        data: updatedResidence,
                        success: function (response) {
                            if (response === 1) {
                                alert('Cập nhật thành công');
                                row.cells[0].innerText = updatedResidence.fullName;
                                row.cells[1].innerText = updatedResidence.dateOfBirth;
                                row.cells[2].innerText = updatedResidence.gender === '1' ? 'Nam' : updatedResidence.gender === '2' ? 'Nữ' : 'Khác';
                                row.cells[3].innerText = updatedResidence.identityNumber;
                                row.cells[4].innerText = updatedResidence.phoneNumber;

                                editForm.remove();
                            } else {
                                alert('Cập nhật thất bại: ' + response.message);
                            }
                        },
                        error: function (xhr, status, error) {
                            alert('Có lỗi xảy ra: ' + error);
                        }
                    });
                });//ajax edit

                document.getElementById('cancelButton').addEventListener('click', function () {
                    editForm.remove();
                });
            });
            //#endregion
            //#region Xóa
            //#region css button "Xóa"
            const deleteButton = document.createElement('button');
            deleteButton.innerText = 'Xóa';
            deleteButton.style.backgroundColor = '#f44336';
            deleteButton.style.color = 'white';
            deleteButton.style.padding = '8px 16px';
            deleteButton.style.marginRight = '8px';
            deleteButton.style.border = 'none';
            deleteButton.style.borderRadius = '4px';
            deleteButton.style.cursor = 'pointer';
            deleteButton.style.fontSize = '14px';
            deleteButton.style.transition = 'background-color 0.3s';
            deleteButton.setAttribute('data-id', item.id);
 
            //#endregion

            //#region thực hiện xóa
            deleteButton.addEventListener('click', function () {
                const id = deleteButton.getAttribute('data-id');
                const confirmation = confirm('Bạn có chắc chắn muốn xóa bản ghi này?');

                if (confirmation) {
                    $.ajax({
                        url: '/ResidenceRegistration/DeleteResidenceRegistration',
                        type: 'delete',
                        data: { id: id },
                        success: function (response) {
                            if (response === 1) {
                                alert('Xóa thành công');
                                const rowToDelete = deleteButton.closest('tr');
                                rowToDelete.remove();

                                dataRessidence = dataRessidence.filter(item => item.id !== id);
                                //temp = dataRessidence.filter(item => item.checkOutTime === null).length;
                                temp = dataRessidence.length;
                                title.innerText = 'Danh sách khách hàng ' + temp + '/' + maximumOccupancy;
                            } else {
                                alert('Xóa thất bại: ' + response.message);
                            }
                        },
                        error: function (xhr, status, error) {
                            alert('Có lỗi xảy ra: ' + error);
                        }
                    });
                }
            });
            //#endregion
            //#endregion
            //#region Rời
            const leaveButton = document.createElement('button');
            leaveButton.innerText = 'Rời';
            leaveButton.style.backgroundColor = '#FF9800'; 
            leaveButton.style.color = 'white';
            leaveButton.style.padding = '8px 16px';
            leaveButton.style.marginRight = '8px';
            leaveButton.style.border = 'none';
            leaveButton.style.borderRadius = '4px';
            leaveButton.style.cursor = 'pointer';
            leaveButton.style.fontSize = '14px';
            leaveButton.style.transition = 'background-color 0.3s';
            leaveButton.setAttribute('data-id', item.id); 

            if (item.checkOutTime) {
                leaveButton.disabled = true; 
                leaveButton.style.backgroundColor = '#D3D3D3';  
                leaveButton.style.cursor = 'default'; 

                deleteButton.disabled = true;
                deleteButton.style.backgroundColor = '#D3D3D3';   
                deleteButton.style.cursor = 'default'; 

                editButton.disabled = true;
                editButton.style.backgroundColor = '#D3D3D3';
                editButton.style.cursor = 'default';
            }

            //thực hiện rời
            leaveButton.addEventListener('click', function () {
                const id = leaveButton.getAttribute('data-id');
                const confirmation = confirm('Bạn có chắc chắn muốn check out khách hàng này?');

                if (confirmation) {
                    $.ajax({
                        url: '/ResidenceRegistration/CheckOut1Residence',
                        type: 'put', 
                        data: { id: id },
                        success: function (response) {
                            if (response === 1) {
                                alert('Check Out thành công');
                                leaveButton.disabled = true;
                                leaveButton.style.backgroundColor = '#D3D3D3';
                                leaveButton.style.cursor = 'default';

                                deleteButton.disabled = true;
                                deleteButton.style.backgroundColor = '#D3D3D3';
                                deleteButton.style.cursor = 'default';

                                editButton.disabled = true;
                                editButton.style.backgroundColor = '#D3D3D3';
                                editButton.style.cursor = 'default';

                                const checkOutTime = moment().format("YYYY-MM-DD HH:mm");
                                row.cells[5].innerText = checkOutTime;

                                dataRessidence = dataRessidence.map(item =>
                                    item.id === id ? { ...item, checkOutTime: new Date().toISOString() } : item
                                );
                            } else {
                                alert('Check Out thất bại: ' + response.message);
                            }
                        },
                        error: function (xhr, status, error) {
                            alert('Có lỗi xảy ra: ' + error);
                        }
                    });
                }
            });
            //#endregion


            actionsTd.appendChild(editButton);
            actionsTd.appendChild(deleteButton);
            actionsTd.appendChild(leaveButton);
            row.appendChild(actionsTd);
            tbody.appendChild(row);
        }); //hết bảng

        table.appendChild(tbody);
        modalContainer.appendChild(table);

        // Append modal to body
        document.body.appendChild(modalContainer);

        // Tạo overlay
        const overlay = document.createElement('div');
        overlay.id = 'customModalOverlay';
        overlay.style.position = 'fixed';
        overlay.style.top = '0';
        overlay.style.left = '0';
        overlay.style.width = '100%';
        overlay.style.height = '100%';
        overlay.style.backgroundColor = 'rgba(0, 0, 0, 0.5)';
        overlay.style.zIndex = '999';
        overlay.onclick = function () {
            modalContainer.remove();
            overlay.remove();
        };

        document.body.appendChild(overlay);
        //#endregion
    },
    getMaximumOccupancyByRoomBookingDetailId: function (Id) {
        return new Promise(function (resolve, reject) {
            $.ajax({
                url: '/ResidenceRegistration/GetMaximumOccupancyByRoomBookingDetailId',
                type: 'POST',
                data: { Id: Id },
                success: function (response) {
                    maximumOccupancy = response.maximumOccupancy;
                    resolve(maximumOccupancy); // Trả về maximumOccupancy sau khi thành công
                },
                error: function (error) {
                    reject(error);  
                }
            });
        });

    },
    getResidenceRegistrations: async function (Id) {
        try {
            // Đợi cho việc lấy maximumOccupancy hoàn thành
            let maximumOccupancy = await _roombooking_detail.getMaximumOccupancyByRoomBookingDetailId(Id);

            $.ajax({
                url: '/ResidenceRegistration/GetResidenceRegistrationByRoomBookingDetailId',
                type: 'POST',
                data: { Id: Id },
                success: function (response) {
                    dataRessidence = response;
                    console.log(response)
                    //residenceCount = response.filter(item => item.checkOutTime === null).length;
                    residenceCount = response.length;

                    // Hiển thị danh sách với maximumOccupancy
                    titlee = 'Danh sách khách hàng ' + residenceCount + '/' + maximumOccupancy;

                    _roombooking_detail.showResidenceModal(response, Id);
                }
            });
        } catch (error) {
            console.error("Error fetching maximumOccupancy:", error);
        }
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

    cancleRoomBookingDetail: function (Id) {
        $("#Status_" + Id).val(3);
        $("#StatusRBD_" + Id).html("Đã hủy");
        $("#StatusRBD_" + Id).css('color', 'red');
        $("#ExtraPr_" + Id).attr('disabled', 'disabled');
        _roombooking_detail.CalculatingRoomPrice()
        $("#btn-huy-" + Id).remove();
    },

    CheckIn: function (Id) {
        $("#Status_" + Id).val(2);
        $("#StatusRBD_" + Id).html("Đã nhận phòng");
        var newDate = global.createNewDateInVietnamTimezone();
        newDate = newDate.toISOString().slice(0, 16);
        $("#StatusRBD_" + Id).css('color', 'green');
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
        _roombooking_detail.CalculatingRoomPrice();

        $.ajax({
            url: '/ResidenceRegistration/CheckOutResideecByRBD',
            type: 'POST',
            data: {roomBookingDetailId : Id},
            success: function (response) {
                console.log(response);
                alert('CheckOut thành công!');
            },
            error: function (xhr, status, error) {
                alert('Có lỗi xảy ra: ' + error);
            }
        });
    },
    OnchangeMinRoom: function () {
        getRoomRq.MinPrice = $("#min_room").val();
    },
    OnchangeMaxRoom: function () {
        getRoomRq.MaxPrice = $("#max_room").val();
    },

    GetlistObjSubmit: function () {
        RoomBooking =
        {
            Id: $("#IdRoomBooking").val(),
            CustomerId: $("#Client_Id").val(),
            Status: roomBookingStatus,
            TotalPriceReality: parseInt($("#total_roomprice").text().replaceAll(',', '')) + parseInt($("#total_service_price").text().replaceAll(',', '')) + parseInt($("#TotalExtraPrice").text().replaceAll(',', '')),
            TotalExtraPrice: parseInt($("#TotalExtraPrice").text().replaceAll(',', '')) + parseInt($("#total_service_extra_price").text().replaceAll(',', '')),
            TotalRoomPrice: parseInt($("#total_roomprice").text().replaceAll(',', '')),
            ToTalExpenses: parseInt($("#TotalExpense").text().replaceAll(',', '')),
            TotalPrice: parseInt($("#TotalStart").text().replaceAll(',', '')),
            TotalServicePrice: parseInt($("#total_service_price").text().replaceAll(',', ''))
        }

        LstIdAdd.forEach(item => {
            var obj =
            {
                RoomId: $("#Idroom_" + item).val(),
                CheckInBooking: $("#CheckIn_" + item).val(),
                CheckOutBooking: $("#CheckOut_" + item).val(),
                CheckInReality: null,
                CheckOutReality: null,
                Price: $("#RoomPr_"+item).text(),
                Status: $("#Status_" + item).val()
            }
            lstRoomBookingDetail.push(obj);
        });
        LstIdUpdate.forEach(item => {
            var obj =
            {
                Id: $("#Id_" + item).val(),
                RoomId: $("#Idroom_" + item).val(),
                CheckInBooking: $("#CheckIn_" + item).val(),
                CheckOutBooking: $("#CheckOut_" + item).val(),
                CheckInReality: $("#CheckInReal_" + item).val(),
                CheckOutReality: $("#CheckOutReal_" + item).val(),
                Price: $("#RoomPr_" + item).text(),
                Status: $("#Status_" + item).val()
            }
            lstRoomBookingDetail.push(obj);
        })
        console.log(RoomBooking)
        console.log(lstRoomBookingDetail)
    },

    submit: async function () {
        const confrm = await global.Noti("Xác nhận cập nhật","Bạn có chắc chắn muốn cập nhật không?");
        if (confrm > 0) {
            _roombooking_detail.GetlistObjSubmit();
            if (lstRoomBookingDetail.length <= 0) {
                lstRoomBookingDetail = [];
                RoomBooking = [];
                Swal.fire({
                    icon: 'error',
                    title: 'Thông báo đặt phòng',
                    text: 'Bạn phải chọn ít nhất 1 phòng.'
                });
            }
            else if (RoomBooking.CustomerId == null) {
                lstRoomBookingDetail = [];
                RoomBooking = [];
                Swal.fire({
                    icon: 'error',
                    title: 'Thông báo đặt phòng',
                    text: 'Bạn chưa nhập thông tin khách hàng.'
                });
            }
            else {
                $.ajax({
                    url: "/RoomBooking/submit",
                    type: "post",
                    data: { bookingcreaterequest: RoomBooking, lstupsert: lstRoomBookingDetail },
                    success: function (result) {
                        window.location.href = result;
                    }
                });
            }
        }
        else {
            lstRoomBookingDetail = [];
            RoomBooking = [];
        }
    },

    BlockedBill: async function (Id) {
        const confrm = await global.Noti("Xác nhận đóng đơn đặt phòng", "<strong>Lưu ý</strong>: Bạn sẽ không thể cập nhật thông tin sau khi đóng đơn\n Bạn có chắc chắn muốn cập nhật không?");
        if (confrm > 0) {
            $.ajax({
                url: "/RoomBooking/BlockedBill",
                type: "post",
                data: { Id: Id, CusId: $("#Client_Id").val() },
                success: function (result) {
                    window.location.href = result;
                }
            });
        }
    }
}