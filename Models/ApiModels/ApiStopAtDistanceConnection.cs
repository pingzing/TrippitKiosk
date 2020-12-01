using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TrippitKiosk.Models.ApiModels
{
    public class ApiStopAtDistanceConnection
    {
        [JsonPropertyName("edges")]
        public List<ApiStopAtDistanceEdge> Edges { get; set; }
        /// <summary>
        /// Non-nullable.
        /// </summary>
        [JsonPropertyName("pageInfo")]
        public ApiPageInfo PageInfo { get; set; }
    }
}
