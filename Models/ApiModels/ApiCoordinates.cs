using System.Text.Json.Serialization;

namespace TrippitKiosk.Models.ApiModels
{
    public struct ApiCoordinates
    {
        [JsonPropertyName("lat")]
        public float Lat { get; set; }
        [JsonPropertyName("lon")]
        public float Lon { get; set; }
    }
}