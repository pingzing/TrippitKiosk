using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TrippitKiosk.Models.ApiModels
{
    public class ApiStoptimesInPattern
    {
        [JsonPropertyName("pattern")]
        public ApiPattern Pattern { get; set; }

        [JsonPropertyName("stoptimes")]
        public List<ApiStoptime> Stoptimes { get; set; }
    }
}