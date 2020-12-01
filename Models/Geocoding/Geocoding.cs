using System.Text.Json.Serialization;

namespace TrippitKiosk.Models.Geocoding
{
    public class Geocoding
    {
        [JsonPropertyName("version")]
        public string Version { get; set; }
        [JsonPropertyName("attribution")]
        public string Attribution { get; set; }
        [JsonPropertyName("query")]
        public GeocodingQuery Query { get; set; }
        [JsonPropertyName("engine")]
        public GeocodingEngine Engine { get; set; }
        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }
    }
}
