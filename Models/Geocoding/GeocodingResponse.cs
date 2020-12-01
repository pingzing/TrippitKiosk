using System.Text.Json.Serialization;

namespace TrippitKiosk.Models.Geocoding
{
    public class GeocodingResponse
    {
        [JsonPropertyName("geocoding")]
        public Geocoding Geocoding { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("features")]
        public GeocodingFeature[] Features { get; set; }
        [JsonPropertyName("bbox")]
        public double[] BoundingBox { get; set; }
    }
}
