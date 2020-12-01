using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TrippitKiosk.Models.ApiModels
{
    public class ApiItinerary
    {
        [JsonPropertyName("startTime")]
        public long? StartTime { get; set; }
        [JsonPropertyName("endTime")]
        public long? EndTime { get; set; }
        [JsonPropertyName("duration")]
        public long? Duration { get; set; }
        [JsonPropertyName("waitingTime")]
        public long WaitingTime { get; set; }
        [JsonPropertyName("walkTime")]
        public long? WalkTime { get; set; }
        [JsonPropertyName("walkDistance")]
        public long? WalkDistance { get; set; }
        [JsonPropertyName("legs")]
        public List<ApiLeg> Legs { get; set; }
        [JsonPropertyName("fares")]
        public List<ApiFare> Fares { get; set; }
    }
}