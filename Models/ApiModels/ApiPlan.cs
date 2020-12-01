using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TrippitKiosk.Models.ApiModels
{
    public class ApiPlan
    {
        [JsonPropertyName("date")]
        public long? Date { get; set; }
        [JsonPropertyName("from")]
        public ApiPlace From { get; set; }
        [JsonPropertyName("to")]
        public ApiPlace To { get; set; }
        [JsonPropertyName("itineraries")]
        public List<ApiItinerary> Itineraries { get; set; }
        [JsonPropertyName("messageEnums")]
        public List<string> MessageEnums { get; set; }
        [JsonPropertyName("messageStrings")]
        public List<string> MessageStrings { get; set; }
        [JsonPropertyName("debugOutput")]
        public ApiDebugOutput? DebugOutput { get; set; }
    }
}
