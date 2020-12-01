using System.Text.Json.Serialization;

namespace TrippitKiosk.Models.Geocoding
{
    public class GeocodingEngine
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("author")]
        public string Author { get; set; }
        [JsonPropertyName("version")]
        public string Version { get; set; }
    }
}