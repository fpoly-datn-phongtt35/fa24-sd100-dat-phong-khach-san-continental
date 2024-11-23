namespace API.Models
{
    public class Data
    {
        public int acpId { get; set; }
        public string accountName { get; set; }
        public string qrCode { get; set; }
        public string qrDataURL { get; set; }
    }

    public class ApiResponse
    {
        public string code { get; set; }
        public string desc { get; set; }
        public Data data { get; set; }
    }
}
