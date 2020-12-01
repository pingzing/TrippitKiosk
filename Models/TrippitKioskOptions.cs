namespace TrippitKiosk.Models
{
    public class TrippitKioskOptions
    {
        public double YouAreHereLat { get; set; }
        public double YouAreHereLon { get; set; }

        public double DefaultCenterLat { get; set; }
        public double DefaultCenterLon { get; set; }

        public string MapApiKey { get; set; }
    }
}
