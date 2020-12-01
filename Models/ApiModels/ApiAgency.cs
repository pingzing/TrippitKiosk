using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TrippitKiosk.Models.ApiModels
{
    public class ApiAgency
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
        /// <summary>
        /// Non-nullable.
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; }
        /// <summary>
        /// Non-nullable.
        /// </summary>
        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }
        [JsonPropertyName("lang")]
        public string Lang { get; set; }
        [JsonPropertyName("phone")]
        public string Phone { get; set; }
        [JsonPropertyName("fareUrl")]
        public string FareUrl { get; set; }
        /// <summary>
        /// Nullable.
        /// </summary>
        [JsonPropertyName("routes")]
        public List<ApiRoute> Routes { get; set; }
        /// <summary>
        /// Nullable.
        /// </summary>
        [JsonPropertyName("alerts")]
        public List<ApiAlert> Alerts { get; set; }
    }
}