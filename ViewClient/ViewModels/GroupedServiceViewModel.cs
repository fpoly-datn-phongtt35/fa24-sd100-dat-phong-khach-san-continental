using Newtonsoft.Json;

namespace ViewClient.ViewModels
{
    public class ApiResponse
    {
        public List<GroupedServiceViewModel> Data { get; set; }
        public int TotalPage { get; set; }
        public int TotalRecord { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }

    public class GroupedServiceViewModel
    {
        [JsonProperty("serviceTypeName")]
        public string ServiceTypeName { get; set; }

        [JsonProperty("serviceIds")]
        public List<Guid> ServiceIds { get; set; }

        [JsonProperty("serviceNames")]
        public List<string> ServiceNames { get; set; }
    }
}
