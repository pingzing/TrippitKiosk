using System.Text.Json.Serialization;

namespace TrippitKiosk.Models.ApiModels
{
    public class ApiStopAtDistance
    {
        /// <summary>
        /// Non-nullable.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("stop")]
        public ApiStop Stop { get; set; }
        [JsonPropertyName("distance")]
        public int Distance { get; set; }
    }
}