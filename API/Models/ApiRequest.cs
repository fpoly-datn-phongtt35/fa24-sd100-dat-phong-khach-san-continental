namespace API.Models
{
    public class ApiRequest
    {
        public long accountNo { get; set; } = 108870026142;
        public string accountName { get; set; } = " VU TUAN ANH";
        public int acqId { get; set; } = 970415; //bin vietinbank
        public int amount { get; set; } //tiền
        public string addInfo { get; set; } //nd chuyển khoản
        public string format { get; set; } = "text";
        public string template { get; set; } = "compact2";
    }
}



//{
//    "accountNo": 108870026142,
//  "accountName": "VU TUAN ANH",
//  "acqId": 970415,
//  "amount": 10000,
//  "addInfo": "string",
//  "format": "text",
//  "template": "compact2"
//}
