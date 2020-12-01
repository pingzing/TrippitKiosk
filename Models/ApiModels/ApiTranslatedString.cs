using System.Text.Json.Serialization;

namespace TrippitKiosk.Models.ApiModels
{
    public class ApiTranslatedString
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonIgnore]
        public Language LanguageAsEnum
        {
            get
            {
                return LanguageEnum.LanguageCodeToLanuage(Language);
            }
        }
    }
}
