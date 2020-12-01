using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TrippitKiosk.Models.ApiModels
{
    public class ApiPattern
    {
        /// <summary>
        /// Non-nullable.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("route")]
        public ApiRoute Route { get; set; }
        [JsonPropertyName("directionId")]
        public int? DirectionId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        /// <summary>
        /// Non-nullable.
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; set; }
        [JsonPropertyName("headsign")]
        public string HeadSign { get; set; }
        /// <summary>
        /// Non-nullable.
        /// </summary>
        [JsonPropertyName("trips")]
        public List<ApiTrip> Trips { get; set; }
        /// <summary>
        /// Non-nullable.
        /// </summary>
        [JsonPropertyName("stops")]
        public List<ApiStop> Stops { get; set; }
        /// <summary>
        /// Nullable.
        /// </summary>
        [JsonPropertyName("geometry")]
        public List<ApiCoordinates> Geometry { get; set; }
        [JsonPropertyName("semanticHash")]
        public string SemanticHash { get; set; }
        /// <summary>
        /// Nullable.
        /// </summary>
        [JsonPropertyName("alerts")]
        public List<ApiAlert> Alerts { get; set; }
    }
}