using System.Text.Json;
using System.Text.Json.Serialization;

namespace TrippitKiosk.Models.ApiModels
{
    public class ApiDataContainer<T>
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }
    }
}
