using Domain.Services.IServices.IRoomBooking;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;
using API.Types;
using System.Text;
using System.Security.Cryptography;
using Domain.Services.IServices;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IRoomBookingGetService _roomBookingGetService;
        private readonly ICustomerService _customerService;
        private readonly IPaymentHistoryService _paymentHistoryService;
        private readonly PayOS _payOS;
        public OrderController(IRoomBookingGetService roomBookingGetService, ICustomerService customerService, IPaymentHistoryService paymentHistoryService, PayOS payOS)
        {
            _roomBookingGetService = roomBookingGetService;
            _customerService = customerService;
            _paymentHistoryService = paymentHistoryService;
            _payOS = payOS;
        }

        public static int GenerateOrderCode(Guid roomBookingId)
        {
            // Chuyển GUID thành chuỗi
            string input = roomBookingId.ToString();

            // Sử dụng SHA256 để băm
            using var sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Lấy 4 byte đầu (32 bit) và chuyển thành int
            int orderCode = BitConverter.ToInt32(hashBytes, 0); // Dùng ToInt32 để lấy 4 byte đầu tiên

            // Đảm bảo giá trị không âm
            return Math.Abs(orderCode);
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreatePaymentLink(Guid roomBookingId, int? deposit)
        {
            var urls = "https://my.payos.vn/ff1f9d29a95211ef964c0242ac110002/payment-link";
            var roomBooking = await _roomBookingGetService.GetRoomBookingById(roomBookingId);
            var customer = await _customerService.GetCustomerById(roomBooking.CustomerId);

            try
            {
                int orderCode = GenerateOrderCode(roomBookingId);
                //int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
                string description = "1";
                ItemData item = new ItemData(roomBooking?.Id.ToString() ?? Guid.Empty.ToString(), 1, deposit ?? (int)(roomBooking.TotalPrice));


                List<ItemData> items = new List<ItemData>() { item };
                PaymentData paymentData = new PaymentData(orderCode, deposit ?? (int)(roomBooking.TotalPrice), description, items,
                    urls, urls);

                CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);

                return Ok(new Response(0, "ok", createPayment));
            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception);
                return Ok(new Response(-1, "fail", null));
            }
        }

        [HttpPost("create-remaining")]
        public async Task<IActionResult> CreateRemainingPaymentLink(Guid paymentHistoryId)
        {
            var urls = "https://my.payos.vn/ff1f9d29a95211ef964c0242ac110002/payment-link";

            var unfinishedPaidRoomBooking = await _paymentHistoryService.GetPaymentHistoryById(paymentHistoryId);
            var roomBooking = await _roomBookingGetService.GetRoomBookingById(unfinishedPaidRoomBooking.RoomBookingId);


            if (roomBooking is not null)
            {
                try
                {
                    int orderCode = GenerateOrderCode(paymentHistoryId);
                    string description = "1";
                    ItemData item = new ItemData(roomBooking?.Id.ToString() ?? Guid.Empty.ToString(), 1, (int)(roomBooking.TotalPrice) - (int)unfinishedPaidRoomBooking.Amount );


                    List<ItemData> items = new List<ItemData>() { item };
                    PaymentData paymentData = new PaymentData(orderCode, (int)(roomBooking.TotalPrice) - (int)unfinishedPaidRoomBooking.Amount, description, items,
                        urls, urls);

                    CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);

                    return Ok(new Response(0, "ok", createPayment));
                }
                catch (System.Exception exception)
                {
                    Console.WriteLine(exception);
                    return Ok(new Response(-1, "fail", null));
                }
            }
            return Ok(new Response(-1, "fail", null));


        }


        [HttpGet("GetOrderInfo/{orderId}")]
        public async Task<IActionResult> GetOrder([FromRoute] int orderId)
        {
            try
            {
                PaymentLinkInformation paymentLinkInformation = await _payOS.getPaymentLinkInformation(orderId);
                return Ok(new Response(0, "Ok", paymentLinkInformation));
            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception);
                return Ok(new Response(-1, "fail", null));
            }
        }

        [HttpGet("GetOrderLink/{orderId}")]
        public async Task<IActionResult> GetOrderLink([FromRoute] int orderId)
        {
            try
            {
                PaymentLinkInformation paymentLinkInformation = await _payOS.getPaymentLinkInformation(orderId);
                var id = paymentLinkInformation.id;
                var paymentLink = $"https://pay.payos.vn/web/{id}";
                return Ok(new Response(0, "Ok", paymentLink));
            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception);
                return Ok(new Response(-1, "fail", null));
            }
        }

        [HttpPut("{orderId}")]
        public async Task<IActionResult> CancelOrder([FromRoute] int orderId)
        {
            try
            {
                PaymentLinkInformation paymentLinkInformation = await _payOS.cancelPaymentLink(orderId);
                return Ok(new Response(0, "Ok", paymentLinkInformation));
            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception);
                return Ok(new Response(-1, "fail", null));
            }
        }
    }
}
