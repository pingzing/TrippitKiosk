using System.Text.Json.Serialization;

namespace TrippitKiosk.Models.Geocoding
{
    public class GeocodingGeometry
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("coordinates")]
        public double[] Coordinates { get; set; }
    }
}