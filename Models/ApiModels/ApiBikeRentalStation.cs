using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TrippitKiosk.Models.ApiModels
{
    public class ApiBikeRentalStation
    {
        /// <summary>
        /// Non-nullable.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("stationId")]
        public string StationId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("bikesAvailable")]
        public int? BikesAvailable { get; set; }
        [JsonPropertyName("spacesAvailable")]
        public int? SpacesAvailable { get; set; }
        [JsonPropertyName("realtime")]
        public bool? Realtime { get; set; }
        [JsonPropertyName("allowDropoff")]
        public bool? AllowDropoff { get; set; }
        /// <summary>
        /// Nullable.
        /// </summary>
        [JsonPropertyName("networks")]
        public List<string> Networks { get; set; }
        [JsonPropertyName("long")]
        public float? Lon { get; set; }
        [JsonPropertyName("lat")]
        public float? Lat { get; set; }
    }
}