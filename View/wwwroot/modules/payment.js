$(document).ready(function () {
    var amountToPay = $('#amount').val();
    console.log(amountToPay);
    if (amountToPay == 0) {
        $('#btn-thanhToanVisible').prop('disabled', 'disabled');
    }
})
var _Payment = {
    Paid: async function () {
        var type = $('select[name="PaymentMethod"]').val();
        var amount = $('input[name="Amount"]').val();
        if (amount != 0) {
            if (type == 'Cash') {
                const confrmPaid = await global.Noti("Xác nhận giao dịch", "Bạn có chắc chắn đã nhận đủ " + global.NumberVNFormated(amount) + " đồng từ khách không?");
                if (confrmPaid > 0) {
                    $(".btn-Thanhtoan").click();
                }
            }
            else {
                $(".btn-Thanhtoan").click();
            }
        } else
        {
            Swal.fire({
                icon: 'info',
                title: 'Thông báo thanh toán',
                text: 'Tạm thời thanh toán đủ.'
            });
        }
    }
}