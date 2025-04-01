using Newtonsoft.Json;

namespace ViewClient.ViewModels
{
    public class TermsViewModel
    {
        public string PolicyTypeTitle { get; set; }
        public List<Guid> PolicyIds { get; set; }
        public List<string> PolicyTitles { get; set; }
        public List<string> PolicyContents { get; set; }
    }
}
