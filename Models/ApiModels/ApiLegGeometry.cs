using System.Text.Json.Serialization;

namespace TrippitKiosk.Models.ApiModels
{
    public struct ApiLegGeometry
    {
        [JsonPropertyName("length")]
        public int Length { get; set; }
        [JsonPropertyName("points")]
        public string Points { get; set; }
    }
}