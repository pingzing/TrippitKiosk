using System.Text.Json.Serialization;

namespace TrippitKiosk.Models.ApiModels
{
    public class ApiStopResponse
    {
        [JsonPropertyName("stop")]
        public ApiStop? Stop { get; set; }
    }
}
