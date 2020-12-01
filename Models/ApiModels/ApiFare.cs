using System.Text.Json.Serialization;

namespace TrippitKiosk.Models.ApiModels
{
    public struct ApiFare
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("currency")]
        public string Currency { get; set; }
        [JsonPropertyName("cents")]
        public int Cents { get; set; }
    }
}