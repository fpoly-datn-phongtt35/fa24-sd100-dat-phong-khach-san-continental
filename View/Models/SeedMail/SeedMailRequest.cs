namespace View.Models.SeedMail
{
    public class SeedMailRequest
    {
        public bool type { get; set; } // true is activating account,false is send code
        public string email { get; set; }
    }
}
