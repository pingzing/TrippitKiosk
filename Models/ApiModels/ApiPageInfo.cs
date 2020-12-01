using System.Text.Json.Serialization;

namespace TrippitKiosk.Models.ApiModels
{
    public class ApiPageInfo
    {
        /// <summary>
        /// Non-nullable.
        /// </summary>
        [JsonPropertyName("hasNextPage")]
        public bool HasNextPage { get; set; }
        /// <summary>
        /// Non-nullable.
        /// </summary>
        [JsonPropertyName("hasPreviousPage")]
        public bool HasPreviousPage { get; set; }
        public string StartCursor { get; set; }
        public string EndCursor { get; set; }
    }
}