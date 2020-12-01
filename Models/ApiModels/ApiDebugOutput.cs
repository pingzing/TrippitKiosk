using System.Text.Json.Serialization;

namespace TrippitKiosk.Models.ApiModels
{
    public struct ApiDebugOutput
    {
        [JsonPropertyName("totalTime")]
        public long? TotalTime { get; set; }
    }
}