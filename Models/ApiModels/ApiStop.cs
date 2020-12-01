using System.Collections.Generic;
using System.Text.Json.Serialization;
using static TrippitKiosk.Models.ApiModels.ApiEnums;

namespace TrippitKiosk.Models.ApiModels
{
    public class ApiStop
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("gtfsId")]
        public string GtfsId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("lat")]
        public float Lat { get; set; }
        [JsonPropertyName("lon")]
        public float Lon { get; set; }
        [JsonPropertyName("code")]
        public string Code { get; set; }
        [JsonPropertyName("desc")]
        public string Desc { get; set; }
        [JsonPropertyName("zoneId")]
        public string ZoneId { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("locationType")]
        public ApiLocationType? LocationType { get; set; }
        /// <summary>
        /// Nullable.
        /// </summary>
        [JsonPropertyName("parentStation")]
        public ApiStop ParentStation { get; set; }
        [JsonPropertyName("wheelchairBoarding")]
        public ApiWheelchairBoarding? WheelchairBoarding { get; set; }
        [JsonPropertyName("direction")]
        public string Direction { get; set; }
        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }
        [JsonPropertyName("vehicleType")]
        public int? VehicleType { get; set; }
        [JsonPropertyName("platformCode")]
        public string PlatformCode { get; set; }
        /// <summary>
        /// Nullable.
        /// </summary>
        [JsonPropertyName("cluster")]
        public ApiCluster Cluster { get; set; }
        /// <summary>
        /// Nullable.
        /// </summary>
        [JsonPropertyName("stops")]
        public List<ApiStop> Stops { get; set; }
        [JsonPropertyName("routes")]
        public List<ApiRoute> Routes { get; set; }
        /// <summary>
        /// Nullable.
        /// </summary>
        [JsonPropertyName("patterns")]
        public List<ApiPattern> Patterns { get; set; }
        /// <summary>
        /// Nullable.
        /// </summary>
        [JsonPropertyName("transfers")]
        public List<ApiStopAtDistance> Transfers { get; set; }
        /// <summary>
        /// Just a container for a <see cref="Trippit.GraphQL.GqlInlineMethodReturnValue"/>. Shouldn't be set directly.
        /// </summary>
        [JsonPropertyName("stoptimesWithoutPatterns")]
        public List<ApiStoptime> StoptimesWithoutPatterns { get; set; }
        /// <summary>
        /// Just a container for a <see cref="GraphQL.GqlInlineMethodReturnValue"/> . Shouldn't be set directly.
        /// </summary>
        [JsonPropertyName("stoptimesForServiceDate")]
        public List<ApiStoptimesInPattern> StoptimesForServiceDate { get; set; }
    }
}