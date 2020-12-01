using System.Text.Json.Serialization;
using static TrippitKiosk.Models.ApiModels.ApiEnums;

namespace TrippitKiosk.Models.ApiModels
{
    public class ApiStoptime
    {
        [JsonPropertyName("stop")]
        public ApiStop Stop { get; set; }
        /// <summary>
        /// Seconds since 00:00 of this day.
        /// </summary>
        [JsonPropertyName("scheduledArrival")]
        public int? ScheduledArrival { get; set; }
        /// <summary>
        /// Seconds since 00:00 of this day.
        /// </summary>
        [JsonPropertyName("realtimeArrival")]
        public int? RealtimeArrival { get; set; }
        [JsonPropertyName("arrivalDelay")]
        public int? ArrivalDelay { get; set; }
        /// <summary>
        /// Seconds since 00:00 of this day.
        /// </summary>
        [JsonPropertyName("scheduledDeparture")]
        public int? ScheduledDeparture { get; set; }
        /// <summary>
        /// Seconds since 00:00 of this day.
        /// </summary>
        [JsonPropertyName("realtimeDeparture")]
        public int? RealtimeDeparture { get; set; }
        [JsonPropertyName("departureDelay")]
        public int? DepartureDelay { get; set; }
        [JsonPropertyName("timepoint")]
        public bool? Timepoint { get; set; }
        [JsonPropertyName("realtime")]
        public bool? Realtime { get; set; }
        [JsonPropertyName("realtimeState")]
        public ApiRealtimeState? RealtimeState { get; set; }
        [JsonPropertyName("pickupType")]
        public ApiPickupDropoffType? PickupType { get; set; }
        [JsonPropertyName("dropoffType")]
        public ApiPickupDropoffType? DropoffType { get; set; }
        [JsonPropertyName("serviceDay")]
        public long? ServiceDay { get; set; }
        [JsonPropertyName("trip")]
        public ApiTrip Trip { get; set; }
        [JsonPropertyName("stopHeadsign")]
        public string StopHeadsign { get; set; }
    }
}