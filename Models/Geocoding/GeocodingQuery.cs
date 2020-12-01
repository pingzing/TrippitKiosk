using System.Text.Json.Serialization;

namespace TrippitKiosk.Models.Geocoding
{
    public class GeocodingQuery
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
        [JsonPropertyName("size")]
        public int Size { get; set; }
        [JsonPropertyName("lang")]
        public string Lang { get; set; }
        [JsonPropertyName("private")]
        public bool IsPrivate { get; set; }
        [JsonPropertyName("focus.point.lat")]
        public double FocusPointLat { get; set; }
        [JsonPropertyName("focus.point.lon")]
        public double FocusPointLon { get; set; }
        [JsonPropertyName("boundary.rect.min_lat")]
        public float BoundaryRectMinLat { get; set; }
        [JsonPropertyName("boundary.rect.max_lat")]
        public float BoundaryRectMaxLat { get; set; }
        [JsonPropertyName("boundary.rect.min_lon")]
        public float BoundaryRectMinLon { get; set; }
        [JsonPropertyName("boundary.rect.max_lon")]
        public float BoundaryRectMaxLon { get; set; }
        [JsonPropertyName("querySize")]
        public int QuerySize { get; set; }
    }
}