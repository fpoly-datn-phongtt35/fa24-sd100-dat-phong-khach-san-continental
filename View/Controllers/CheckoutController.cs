using Microsoft.AspNetCore.Mvc;

namespace View.Controllers;

public class CheckoutController : Controller
{
    [HttpGet("payment/success")]
    public IActionResult PaymentSuccess(long orderCode)
    {
        // Gửi một yêu cầu ngầm để cập nhật dữ liệu qua HandlePaymentCallBack
        ViewBag.OrderCode = orderCode;
        return View("PaymentSuccess");
    }

    [HttpGet("payment/cancel")]
    public IActionResult PaymentCancel(long orderCode)
    {
        // Gửi một yêu cầu ngầm để cập nhật dữ liệu qua HandlePaymentCallBack
        ViewBag.OrderCode = orderCode;
        return View("PaymentCancel");
    }
}