using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TrippitKiosk.Models.ApiModels
{
    public class ApiAlert
    {
        /// <summary>
        /// Non-nullable.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("agency")]
        public ApiAgency Agency { get; set; }
        [JsonPropertyName("route")]
        public ApiRoute Route { get; set; }
        [JsonPropertyName("trip")]
        public ApiTrip Trip { get; set; }
        [JsonPropertyName("stop")]
        public ApiStop Stop { get; set; }
        [JsonPropertyName("patterns")]
        public List<ApiPattern> Patterns { get; set; }
        [JsonPropertyName("alertHeaderText")]
        public string AlertHeaderText { get; set; }
        /// <summary>
        /// Non-nullable.
        /// </summary>
        [JsonPropertyName("alertHeaderTextTranslations")]
        public List<ApiTranslatedString> AlertHeaderTextTranslations { get; set; }
        /// <summary>
        /// Non-nullable.
        /// </summary>
        [JsonPropertyName("alertDescriptionText")]
        public string AlertDescriptionText { get; set; }
        /// <summary>
        /// Non-nullable.
        /// </summary>
        [JsonPropertyName("alertDescriptionTextTranslations")]
        public List<ApiTranslatedString> AlertDescriptionTextTranslations { get; set; }
        [JsonPropertyName("alertUrl")]
        public string AlertUrl { get; set; }
        [JsonPropertyName("effectiveStartDate")]
        public long? EffectiveStartDate { get; set; }
        [JsonPropertyName("effectiveEndDate")]
        public long? EffectiveEndDate { get; set; }
    }
}