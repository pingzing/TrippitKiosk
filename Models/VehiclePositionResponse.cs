#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TrippitKiosk.Models
{
    public class VehiclePositionResponse
    {
        [JsonPropertyName("VP")]
        public VehiclePosition VehiclePosition { get; set; } = null!;
    }

    public class VehiclePosition
    {
        [JsonPropertyName("desi")]
        public string? FriendlyRouteNumber { get; set; }

        [JsonPropertyName("dir")]
        public string? RawDirection { get; set; }

        [JsonIgnore]
        public RouteDirection? Direction => RawDirection switch
        {
            "1" => RouteDirection.TowardEnd,
            "2" => RouteDirection.TowardStart,
            _ => null
        };

        [JsonPropertyName("veh")]
        public int VehicleNumber { get; set; }

        // Timestamp in ISO 8061 format in UTC with ms precision.
        [JsonPropertyName("tst")]
        public DateTimeOffset Timestamp { get; set; }

        // Timestamp in Unix seconds.
        [JsonPropertyName("tsi")]
        public int? UnixTimestamp { get; set; }

        [JsonPropertyName("spd")]
        public double? SpeedMetersPerSec { get; set; }

        [JsonPropertyName("hdg")]
        public int? HeadingDegrees { get; set; }

        [JsonPropertyName("lat")]
        public double? Latitude { get; set; }

        [JsonPropertyName("long")]
        public double? Longitude { get; set; }

        [JsonPropertyName("route")]
        public string? Route { get; set; } = null!;
    }

    public enum RouteDirection
    {
        TowardEnd = 0,
        TowardStart = 1,
    }
}
