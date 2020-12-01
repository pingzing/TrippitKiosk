using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TrippitKiosk.Models.ApiModels
{
    public class ApiCluster
    {
        /// <summary>
        /// Non-nullable.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }
        /// <summary>
        /// Non-nullable.
        /// </summary>
        [JsonPropertyName("gtfsId")]
        public string GtfsId { get; set; }
        /// <summary>
        /// Non-nullable.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("lat")]
        public float Lat { get; set; }
        [JsonPropertyName("lon")]
        public float Lon { get; set; }
        [JsonPropertyName("stops")]
        public List<ApiStop> Stops { get; set; }
    }
}