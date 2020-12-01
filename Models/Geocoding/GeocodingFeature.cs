using System.Text.Json.Serialization;

namespace TrippitKiosk.Models.Geocoding
{
    public class GeocodingFeature
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("geometry")]
        public GeocodingGeometry Geometry { get; set; }
        [JsonPropertyName("properties")]
        public GeocodingProperties Properties { get; set; }
    }
}