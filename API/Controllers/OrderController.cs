using Domain.Services.IServices.IRoomBooking;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;
using API.Types;
using System.Text;
using System.Security.Cryptography;
using Domain.Services.IServices;
using Domain.Enums;
using Domain.DTO.PaymentHistory;
using Domain.DTO.Order;


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


        [HttpPost("create")] // create cho bên client
        public async Task<IActionResult> CreatePaymentLink(PaymentLinkCreateRequest request)
        {
            var cancelUrl = "https://localhost:7130/api/PaymentHistory/payment/callback-refactor";
            var successUrl = "https://localhost:7130/api/PaymentHistory/payment/callback-refactor";
            var roomBooking = await _roomBookingGetService.GetRoomBookingById(request.RoomBookingId);
            

            //var customer = await _customerService.GetCustomerById(roomBooking.CustomerId);

            try
            {
                //int orderCode = GenerateOrderCode(roomBookingId);
                int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
                string description = "1";

                int amount = request.PaymentType switch
                {
                    PaymentType.Bill => request.Money ?? 0,
                    PaymentType.Deposit => (int)(roomBooking.TotalRoomPrice * 20 / 100)
                };
                ItemData item = new ItemData(roomBooking?.Id.ToString() ?? Guid.Empty.ToString(), 1, amount);


                List<ItemData> items = new List<ItemData>() { item };
                PaymentData paymentData = new PaymentData(orderCode, (int)amount, description, items,
                    cancelUrl, successUrl);

                CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);

                // Nếu tạo liên kết thanh toán thành công
                if (createPayment != null && !string.IsNullOrEmpty(createPayment.checkoutUrl))
                {
                    var paymentHistory = new PaymentHistoryCreateRequest
                    {
                        OrderCode = orderCode,
                        RoomBookingId = request.RoomBookingId,
                        Amount = 0,
                        PaymentTime = DateTime.Now,
                        Note = request.PaymentType,
                        PaymentMethod = (PaymentMethod)1
                    };

                    await _paymentHistoryService.AddPaymentHistory(paymentHistory);
                    return Ok(new Response(0, "success", createPayment.checkoutUrl));
                }
                else
                {
                    return Ok(new Response(-1, "fail", null));
                }
            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception);
                return Ok(new Response(-1, "fail", null));
            }
        }


        [HttpPost("admin-create")] 
        public async Task<IActionResult> CreatePaymentLinkAdmin(PaymentLinkCreateRequest request)
        {           
            var roomBooking = await _roomBookingGetService.GetRoomBookingById(request.RoomBookingId);
            var cancelUrl = $"https://localhost:7114/PaymentHistory/Id={request.RoomBookingId}";
            var successUrl = $"https://localhost:7114/PaymentHistory/Id={request.RoomBookingId}";

            //var customer = await _customerService.GetCustomerById(roomBooking.CustomerId);

            try
            {
                //int orderCode = GenerateOrderCode(roomBookingId);
                int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
                string description = "1";

                int amount = request.PaymentType switch
                {
                    PaymentType.Bill => request.Money ?? 0,
                    PaymentType.Deposit => (int)(roomBooking.TotalRoomPrice * 20 / 100)
                };
                ItemData item = new ItemData(roomBooking?.Id.ToString() ?? Guid.Empty.ToString(), 1, amount);


                List<ItemData> items = new List<ItemData>() { item };
                PaymentData paymentData = new PaymentData(orderCode, (int)amount, description, items,
                    cancelUrl, successUrl);

                CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);

                // Nếu tạo liên kết thanh toán thành công
                if (createPayment != null && !string.IsNullOrEmpty(createPayment.checkoutUrl))
                {
                    var paymentHistory = new PaymentHistoryCreateRequest
                    {
                        OrderCode = orderCode,
                        RoomBookingId = request.RoomBookingId,
                        Amount = 0,
                        PaymentTime = DateTime.Now,
                        Note = request.PaymentType,
                        PaymentMethod = (PaymentMethod)1
                    };

                    await _paymentHistoryService.AddPaymentHistory(paymentHistory);
                    return Ok(new Response(0, "success", createPayment.checkoutUrl));
                }
                else
                {
                    return Ok(new Response(-1, "fail", null));
                }
            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception);
                return Ok(new Response(-1, "fail", null));
            }
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

        [HttpPut("CancelOrder{orderId}")]
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
