using System;
using System.Text.Json.Serialization;
using Windows.Devices.Geolocation;
using static TrippitKiosk.Models.ApiModels.ApiEnums;

namespace TrippitKiosk.Models.ApiModels
{
    public class ApiPlace : IMapPoi
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("vertexType")]
        public ApiVertexType? VertexType { get; set; }
        [JsonPropertyName("lat")]
        public float Lat { get; set; }
        [JsonPropertyName("lon")]
        public float Lon { get; set; }
        /// <summary>
        /// Nullable.
        /// </summary>
        [JsonPropertyName("stop")]
        public ApiStop Stop { get; set; }
        /// <summary>
        /// Nullable.
        /// </summary>
        [JsonPropertyName("bikeRentalStation")]
        public ApiBikeRentalStation BikeRentalStation { get; set; }

        [JsonIgnore]
        public BasicGeoposition Coords => new BasicGeoposition { Altitude = 0.0, Latitude = Lat, Longitude = Lon };

        [JsonIgnore]
        public Guid Id { get; set; }
    }
}