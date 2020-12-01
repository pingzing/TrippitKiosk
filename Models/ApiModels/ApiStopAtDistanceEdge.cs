using System.Text.Json.Serialization;

namespace TrippitKiosk.Models.ApiModels
{
    public class ApiStopAtDistanceEdge
    {
        [JsonPropertyName("node")]
        public ApiStopAtDistance Node { get; set; }
        /// <summary>
        /// Non-nullable.
        /// </summary>
        [JsonPropertyName("cursor")]
        public string Cursor { get; set; }
    }
}