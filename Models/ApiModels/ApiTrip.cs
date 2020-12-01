using System.Collections.Generic;
using System.Text.Json.Serialization;
using static TrippitKiosk.Models.ApiModels.ApiEnums;

namespace TrippitKiosk.Models.ApiModels
{
    public class ApiTrip
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
        [JsonPropertyName("route")]
        public ApiRoute Route { get; set; }
        [JsonPropertyName("serviceId")]
        public string ServiceId { get; set; }
        /// <summary>
        /// Nullable.
        /// </summary>
        [JsonPropertyName("activeDates")]
        public List<string> ActiveDates { get; set; }
        [JsonPropertyName("tripShortName")]
        public string TripShortName { get; set; }
        [JsonPropertyName("tripHeadsign")]
        public string TripHeadsign { get; set; }
        [JsonPropertyName("routeShortName")]
        public string RouteShortName { get; set; }
        [JsonPropertyName("directionId")]
        public string DirectionId { get; set; }
        [JsonPropertyName("blockId")]
        public string BlockId { get; set; }
        [JsonPropertyName("shapeId")]
        public string ShapeId { get; set; }
        [JsonPropertyName("wheelchairAccessible")]
        public ApiWheelchairBoarding? WheelchairAccessible { get; set; }
        [JsonPropertyName("bikesAllowed")]
        public ApiBikesAllowed? BikesAllowed { get; set; }
        /// <summary>
        /// Nullable.
        /// </summary>
        [JsonPropertyName("pattern")]
        public ApiPattern Pattern { get; set; }
        [JsonPropertyName("stops")]
        public List<ApiStop> Stops { get; set; }
        [JsonPropertyName("semanticHash")]
        public List<ApiStop> SemanticHash { get; set; }
        /// <summary>
        /// Nullable.
        /// </summary>
        [JsonPropertyName("stoptimes")]
        public List<ApiStoptime> Stoptimes { get; set; }
        /// <summary>
        /// Nullable.
        /// </summary>
        [JsonPropertyName("geometry")]
        public List<float[]> Geometry { get; set; }
        /// <summary>
        /// Nullable.
        /// </summary>
        [JsonPropertyName("alerts")]
        public List<ApiAlert> Alerts { get; set; }
    }
}