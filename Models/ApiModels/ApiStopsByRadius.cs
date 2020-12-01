using System.Text.Json.Serialization;

namespace TrippitKiosk.Models.ApiModels
{
    public class ApiStopsByRadius
    {
        [JsonPropertyName("stopsByRadius")]
        public ApiStopAtDistanceConnection? StopsByRadius { get; set; } = null!;
    }
}
