using System.Text.Json.Serialization;

namespace TrippitKiosk.Models.Geocoding
{
    public class GeocodingProperties
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("gid")]
        public string Gid { get; set; }
        [JsonPropertyName("layer")]
        public string Layer { get; set; }
        [JsonPropertyName("source")]
        public string Source { get; set; }
        [JsonPropertyName("source_id")]
        public string SourceId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("housenumber")]
        public string HouseNumber { get; set; }
        [JsonPropertyName("street")]
        public string Street { get; set; }
        [JsonPropertyName("confidence")]
        public double Confidence { get; set; }
        [JsonPropertyName("distance")]
        public double Distance { get; set; }
        [JsonPropertyName("country")]
        public string Country { get; set; }
        [JsonPropertyName("country_gid")]
        public string CountryGid { get; set; }
        [JsonPropertyName("country_a")]
        public string CountryA { get; set; }
        [JsonPropertyName("macroregion")]
        public string MacroRegion { get; set; }
        [JsonPropertyName("macroregion_gid")]
        public string MacroRegionGid { get; set; }
        [JsonPropertyName("locality")]
        public string Locality { get; set; }
        [JsonPropertyName("locality_gid")]
        public string LocalityGid { get; set; }
        [JsonPropertyName("neighborhood")]
        public string Neighborhood { get; set; }
        [JsonPropertyName("neighborhood_gid")]
        public string NeighborhoodGid { get; set; }
        [JsonPropertyName("label")]
        public string Label { get; set; }
    }
}