using System.Collections.Generic;
using System.Text.Json.Serialization;
using static TrippitKiosk.Models.ApiModels.ApiEnums;

namespace TrippitKiosk.Models.ApiModels
{
    public class ApiLeg
    {
        [JsonPropertyName("startTime")]
        public long? StartTime { get; set; }
        [JsonPropertyName("endTime")]
        public long? EndTime { get; set; }
        [JsonPropertyName("mode")]
        public ApiMode? Mode { get; set; }
        [JsonPropertyName("duration")]
        public long? Duration { get; set; }
        [JsonPropertyName("legGeometry")]
        public ApiLegGeometry LegGeometry { get; set; }
        [JsonPropertyName("agency")]
        public ApiAgency Agency { get; set; }
        [JsonPropertyName("realtime")]
        public bool? Realtime { get; set; }
        [JsonPropertyName("distance")]
        public float? Distance { get; set; }
        [JsonPropertyName("transitLeg")]
        public bool? TransitLeg { get; set; }
        [JsonPropertyName("rentedBike")]
        public bool? RentedBike { get; set; }
        [JsonPropertyName("from")]
        public ApiPlace From { get; set; }
        [JsonPropertyName("to")]
        public ApiPlace To { get; set; }
        [JsonPropertyName("route")]
        public ApiRoute Route { get; set; }
        [JsonPropertyName("trip")]
        public ApiTrip Trip { get; set; }
        [JsonPropertyName("intermediateStops")]
        public List<ApiStop> IntermediateStops { get; set; }
    }
}