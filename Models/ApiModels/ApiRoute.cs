using System.Collections.Generic;
using System.Text.Json.Serialization;
using static TrippitKiosk.Models.ApiModels.ApiEnums;

namespace TrippitKiosk.Models.ApiModels
{
    public class ApiRoute
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
        /// Nullable.
        /// </summary>
        [JsonPropertyName("agency")]
        public ApiAgency Agency { get; set; }
        [JsonPropertyName("shortName")]
        public string ShortName { get; set; }
        [JsonPropertyName("longName")]
        public string LongName { get; set; }
        [JsonPropertyName("mode")]
        public ApiMode Mode { get; set; }
        [JsonPropertyName("desc")]
        public string Desc { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("color")]
        public string Color { get; set; }
        [JsonPropertyName("textColor")]
        public string TextColor { get; set; }
        [JsonPropertyName("bikesAllowed")]
        public ApiBikesAllowed? BikesAllowed { get; set; }
        /// <summary>
        /// Nullable.
        /// </summary>
        [JsonPropertyName("patterns")]
        public List<ApiPattern> Patterns { get; set; }
        /// <summary>
        /// Nullable.
        /// </summary>
        [JsonPropertyName("stops")]
        public List<ApiStop> Stops { get; set; }
        /// <summary>
        /// Nullable.
        /// </summary>
        [JsonPropertyName("trips")]
        public List<ApiTrip> Trips { get; set; }
        /// <summary>
        /// Nullable.
        /// </summary>
        [JsonPropertyName("alerts")]
        public List<ApiAlert> Alerts { get; set; }
    }
}