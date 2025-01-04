using Newtonsoft.Json;

namespace ViewClient.ViewModels
{
    public class TermsViewModel
    {
        public string PostTypeTitle { get; set; }
        public List<Guid> PostIds { get; set; }
        public List<string> PostTitles { get; set; }
        public List<string> PostContents { get; set; }
    }
}
