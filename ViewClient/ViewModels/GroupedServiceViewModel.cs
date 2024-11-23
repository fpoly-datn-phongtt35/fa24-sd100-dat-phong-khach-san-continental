using Newtonsoft.Json;

namespace ViewClient.ViewModels
{
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
